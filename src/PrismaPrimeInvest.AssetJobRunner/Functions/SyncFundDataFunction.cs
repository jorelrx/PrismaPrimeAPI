using System.Globalization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.AssetJobRunner.Models;
using PrismaPrimeInvest.AssetJobRunner.Services;
using PrismaPrimeInvest.Domain.Enums;

namespace PrismaPrimeInvest.AssetJobRunner.Functions;

public class SyncFundDataFunction(
    ILoggerFactory loggerFactory,
    IFundService fundService,
    FundDailyPriceSyncService dailyPriceSyncService,
    AssetHttpService assetHttpService)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<SyncFundDataFunction>();
    private readonly IFundService _fundService = fundService;
    private readonly FundDailyPriceSyncService _dailyPriceSyncService = dailyPriceSyncService;
    private readonly AssetHttpService _assetHttpService = assetHttpService;

    [Function("SyncFundData")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sync-fund-data/{ticker}")] HttpRequestData req,
        string ticker)
    {
        _logger.LogInformation($"Starting sync for fund: {ticker}");

        try
        {
            var fund = await _fundService.GetByCodeAsync(ticker);
            if (fund == null)
            {
                _logger.LogWarning($"Fund not found for ticker: {ticker}");
                var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync($"Asset with ticker '{ticker}' not found.");
                return notFoundResponse;
            }

            int assetType = fund.Type == FundTypeEnum.Fiagro ? 1 : 2;
            IEnumerable<DailyPriceResponse>? dailyPrices = await _assetHttpService.GetDailyPricesByTickerAsync(ticker, assetType);

            if (dailyPrices == null || !dailyPrices.Any())
            {
                _logger.LogWarning($"No data returned from StatusInvest for fund: {ticker}");
                var noDataResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                await noDataResponse.WriteStringAsync($"No data found for fund '{ticker}' from StatusInvest.");
                return noDataResponse;
            }

            _logger.LogWarning($"Count dailyPrices: {dailyPrices.Count()}");

            List<CreateFundDailyPriceDto>? createFundDailyPriceDtos = dailyPrices?.SelectMany(d => d.Prices.Select(p => new CreateFundDailyPriceDto
            {
                Date = p.DateConterted,
                OpenPrice = p.Price,
                ClosePrice = p.Price,
                MaxPrice = p.Price,
                MinPrice = p.Price
            })).ToList();

            _logger.LogWarning($"Count createFundDailyPriceDtos: {createFundDailyPriceDtos?.Count}.");

            if (createFundDailyPriceDtos == null || createFundDailyPriceDtos.Count == 0)
            {
                _logger.LogWarning($"No daily prices found for fund: {ticker}");
                var noDataResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                await noDataResponse.WriteStringAsync($"No daily prices found for fund '{ticker}'.");
                return noDataResponse;
            }
            
            var filledDailyPrices = FillMissingDays(createFundDailyPriceDtos);

            await _dailyPriceSyncService.SyncFundDailyPrices(fund, filledDailyPrices);

            _logger.LogInformation($"{ticker} Asset Prices Updated.");

            var successResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await successResponse.WriteStringAsync($"Fund daily prices for '{ticker}' synced successfully.");

            return successResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error syncing fund daily prices for {ticker}: {ex}", ex);
            var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync($"Error: {ex.Message}");
            return errorResponse;
        }
    }

    private static List<CreateFundDailyPriceDto> FillMissingDays(List<CreateFundDailyPriceDto> dailyPrices)
    {
        if (dailyPrices == null || dailyPrices.Count == 0) return [];

        var filledPrices = new List<CreateFundDailyPriceDto>();
        var orderedPrices = dailyPrices.OrderBy(dp => dp.Date).ToList();

        for (int i = 0; i < orderedPrices.Count - 1; i++)
        {
            var currentPrice = orderedPrices[i];
            filledPrices.Add(currentPrice);

            var nextPrice = orderedPrices[i + 1];
            var daysDiff = (nextPrice.Date - currentPrice.Date).Days;

            for (int j = 1; j < daysDiff; j++)
            {
                filledPrices.Add(new CreateFundDailyPriceDto
                {
                    Date = currentPrice.Date.AddDays(j),
                    OpenPrice = currentPrice.OpenPrice,
                    ClosePrice = currentPrice.ClosePrice,
                    MaxPrice = currentPrice.MaxPrice,
                    MinPrice = currentPrice.MinPrice,
                    FundId = currentPrice.FundId
                });
            }
        }

        filledPrices.Add(orderedPrices.Last());

        return filledPrices;
    }
}
