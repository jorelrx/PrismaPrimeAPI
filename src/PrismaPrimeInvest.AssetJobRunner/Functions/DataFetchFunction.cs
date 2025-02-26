using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Domain.Enums;
using PrismaPrimeInvest.AssetJobRunner.Services;
using System.Text.Json;
using PrismaPrimeInvest.AssetJobRunner.Models;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.Responses;

namespace PrismaPrimeInvest.AssetJobRunner.Functions
{
    public class DataFetchFunction(StatusInvestService statusInvestService, ILoggerFactory loggerFactory, IFundService fundService, IFundDailyPriceService fundDailyPriceService, IFundPaymentService fundPaymentService)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<DataFetchFunction>();
        private readonly StatusInvestService _statusInvestService = statusInvestService;
        private readonly IFundService _fundService = fundService;
        private readonly IFundDailyPriceService _fundDailyPriceService = fundDailyPriceService;
        private readonly IFundPaymentService _fundPaymentService = fundPaymentService;

        [Function("DataFetch")]
        public async Task Run([TimerTrigger("0 */10 10-17 * * 1-5")] TimerInfo myTimer)
        {
            _logger.LogInformation($"DataFetch function executed at: {DateTime.Now}");

            var assets = await _fundService.GetAllAsync(new FilterFund());

            foreach (var asset in assets.Items)
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
                                OpenPrice = result.FirstPrice,
                                ClosePrice = result.CurrentPrice,
                                MaxPrice = result.MaxPrice,
                                MinPrice = result.MinPrice,
                                Date = result.CurrentDate
                            };

                            PagedResult<FundDailyPriceDto>? fundDailyPrices = await _fundDailyPriceService.GetAllAsync(new FilterFundDailyPrice() 
                            {
                                Date = createFundDailyPriceDto.Date,
                                FundId = createFundDailyPriceDto.FundId
                            });

                            _logger.LogInformation($"Quantidade de fundos com a data: {createFundDailyPriceDto.Date.Date} e o FundId: {createFundDailyPriceDto.FundId} => {fundDailyPrices.Items.Count}");

                            FundDailyPriceDto? fundDailyPrice = fundDailyPrices.Items.FirstOrDefault();

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

                            _logger.LogInformation($"Atualizando dados do fundo {asset.Code}.");

                            var fundPayments = await _fundPaymentService.GetAllAsync(new() { FundId = asset.Id, Period = 0 });
                            var dividendSum = fundPayments.Items.Sum(x => x.Dividend);
                            var dividendYield = dividendSum / createFundDailyPriceDto.ClosePrice;
                            
                            UpdateFundDto updateFundDto = new()
                            {
                                Id = asset.Id,
                                Code = asset.Code,
                                Name = asset.Name,
                                Type = asset.Type,
                                Price = createFundDailyPriceDto.ClosePrice,
                                MaxPrice = Math.Max(createFundDailyPriceDto.MaxPrice, asset.MaxPrice),
                                MinPrice = Math.Min(createFundDailyPriceDto.MinPrice, asset.MinPrice),
                                DividendYield = dividendYield
                            };

                            await _fundService.UpdateAsync(asset.Id, updateFundDto);
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

                            PagedResult<FundDailyPriceDto>? fundDailyPrices = await _fundDailyPriceService.GetAllAsync(new FilterFundDailyPrice() 
                            {
                                Date = createFundDailyPriceDto.Date,
                                FundId = createFundDailyPriceDto.FundId
                            });

                            _logger.LogInformation($"Quantidade de fundos com a data: {createFundDailyPriceDto.Date.Date} e o FundId: {createFundDailyPriceDto.FundId} => {fundDailyPrices.Items.Count}");

                            FundDailyPriceDto? fundDailyPrice = fundDailyPrices.Items.FirstOrDefault();

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
                            
                            _logger.LogInformation($"Atualizando dados do fundo {asset.Code}.");

                            var fundPayments = await _fundPaymentService.GetAllAsync(new() { FundId = asset.Id, Period = 0 });
                            var dividendSum = fundPayments.Items.Sum(x => x.Dividend);
                            var dividendYield = dividendSum / createFundDailyPriceDto.ClosePrice;
                            
                            UpdateFundDto updateFundDto = new()
                            {
                                Id = asset.Id,
                                Code = asset.Code,
                                Name = asset.Name,
                                Type = asset.Type,
                                Price = createFundDailyPriceDto.ClosePrice,
                                MaxPrice = Math.Max(createFundDailyPriceDto.MaxPrice, asset.MaxPrice),
                                MinPrice = Math.Min(createFundDailyPriceDto.MinPrice, asset.MinPrice),
                                DividendYield = dividendYield
                            };

                            await _fundService.UpdateAsync(asset.Id, updateFundDto);
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
