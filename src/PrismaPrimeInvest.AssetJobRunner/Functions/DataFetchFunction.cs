using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Domain.Enums;
using PrismaPrimeInvest.AssetJobRunner.Services;
using System.Text.Json;
using PrismaPrimeInvest.AssetJobRunner.Models;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;

namespace PrismaPrimeInvest.AssetJobRunner.Functions
{
    public class DataFetchFunction(StatusInvestService statusInvestService, ILoggerFactory loggerFactory, IFundService fundService, IFundDailyPriceService fundDailyPriceService)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<DataFetchFunction>();
        private readonly StatusInvestService _statusInvestService = statusInvestService;
        private readonly IFundService _fundService = fundService;
        private readonly IFundDailyPriceService _fundDailyPriceService = fundDailyPriceService;

        [Function("DataFetch")]
        public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"DataFetch function executed at: {DateTime.Now}");

            var assets = await _fundService.GetAllAsync(new FilterFund());

            foreach (var asset in assets)
            {
                try
                {
                    if ((FundTypeEnum)Enum.Parse(typeof(FundTypeEnum), asset.Type) == FundTypeEnum.Fiagro)
                    {
                        string? jsonResponse = await _statusInvestService.PostTickerPriceAsync(asset.Code, "https://statusinvest.com.br/fiagro/tickerprice");
                        List<TickerPriceResponse>? data = JsonSerializer.Deserialize<List<TickerPriceResponse>>(jsonResponse, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        TickerPriceResponse? result = data?.FirstOrDefault();

                        if (result != null)
                        {
                            _logger.LogInformation($"Adicionando novo registro do fundo {asset.Code}");
                            CreateFundDailyPriceDto createFundDailyPriceDto = new()
                            {
                                FundId = asset.Id,
                                OpenPrice = result.CurrentPrice,
                                ClosePrice = result.CurrentPrice,
                                MaxPrice = result.MaxPrice,
                                MinPrice = result.MinPrice,
                                Date = result.CurrentDate
                            };

                            List<FundDailyPriceDto>? fundDailyPrices = await _fundDailyPriceService.GetAllAsync(new FilterFundDailyPrice() 
                            {
                                Date = createFundDailyPriceDto.Date,
                                FundId = createFundDailyPriceDto.FundId
                            });

                            _logger.LogInformation($"Quantidade de fundos com a data: {createFundDailyPriceDto.Date.Date} e o FundId: {createFundDailyPriceDto.FundId} => {fundDailyPrices.Count}");

                            FundDailyPriceDto? fundDailyPrice = fundDailyPrices.FirstOrDefault();

                            if (fundDailyPrice == null)
                            {
                                Guid createFundDailyPriceId = await _fundDailyPriceService.CreateAsync(createFundDailyPriceDto);
                                _logger.LogInformation($"Novo registro diario do fundo {asset.Code} e data {createFundDailyPriceDto.Date} criado com o ID => {createFundDailyPriceId}.");
                            }
                            else
                            {
                                _logger.LogWarning($"Alterando registro diario do fundo {asset.Code} e data {createFundDailyPriceDto.Date}.");
                                await _fundDailyPriceService.UpdateAsync(fundDailyPrice.Id, new() {
                                    Id = fundDailyPrice.Id,
                                    Code = fundDailyPrice.Code,
                                    Name = fundDailyPrice.Name,
                                    Type = fundDailyPrice.Type,
                                    OpenPrice = fundDailyPrice.OpenPrice,
                                    ClosePrice = createFundDailyPriceDto.ClosePrice,
                                    MaxPrice = createFundDailyPriceDto.MaxPrice,
                                    MinPrice = createFundDailyPriceDto.MinPrice
                                });
                            }
                        }
                        else
                        {
                            _logger.LogWarning("No data returned for ticker.");
                        }
                    }
                    else
                    {
                        string? jsonResponse = await _statusInvestService.PostTickerPriceAsync(asset.Code, "https://statusinvest.com.br/fii/tickerprice");
                        List<TickerPriceResponse>? data = JsonSerializer.Deserialize<List<TickerPriceResponse>>(jsonResponse, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        TickerPriceResponse? result = data?.FirstOrDefault();

                        if (result != null)
                        {
                            CreateFundDailyPriceDto createFundDailyPriceDto = new()
                            {
                                FundId = asset.Id,
                                OpenPrice = result.CurrentPrice,
                                ClosePrice = result.CurrentPrice,
                                MaxPrice = result.MaxPrice,
                                MinPrice = result.MinPrice,
                                Date = result.CurrentDate
                            };

                            List<FundDailyPriceDto>? fundDailyPrices = await _fundDailyPriceService.GetAllAsync(new FilterFundDailyPrice() 
                            {
                                Date = createFundDailyPriceDto.Date,
                                FundId = createFundDailyPriceDto.FundId
                            });

                            _logger.LogInformation($"Quantidade de fundos com a data: {createFundDailyPriceDto.Date.Date} e o FundId: {createFundDailyPriceDto.FundId} => {fundDailyPrices.Count}");

                            FundDailyPriceDto? fundDailyPrice = fundDailyPrices.FirstOrDefault();

                            if (fundDailyPrice == null)
                            {
                                _logger.LogInformation($"Adicionando novo registro do fundo {asset.Code}");
                                Guid createFundDailyPriceId = await _fundDailyPriceService.CreateAsync(createFundDailyPriceDto);
                                _logger.LogInformation($"Novo registro diario do fundo {asset.Code} e data {createFundDailyPriceDto.Date} criado com o ID => {createFundDailyPriceId}.");
                            }
                            else
                            {
                                _logger.LogWarning($"Alterando registro diario do fundo {asset.Code} e data {createFundDailyPriceDto.Date}.");
                                await _fundDailyPriceService.UpdateAsync(fundDailyPrice.Id, new() {
                                    Id = fundDailyPrice.Id,
                                    Code = fundDailyPrice.Code,
                                    Name = fundDailyPrice.Name,
                                    Type = fundDailyPrice.Type,
                                    OpenPrice = fundDailyPrice.OpenPrice,
                                    ClosePrice = createFundDailyPriceDto.ClosePrice,
                                    MaxPrice = createFundDailyPriceDto.MaxPrice,
                                    MinPrice = createFundDailyPriceDto.MinPrice
                                });
                            }
                        }
                        else
                        {
                            _logger.LogWarning("No data returned for ticker.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.Message);
                }
            }
        }
    }
}
