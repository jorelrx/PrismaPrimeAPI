using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.AssetJobRunner.Models;
using PrismaPrimeInvest.Domain.Entities.Invest;

namespace PrismaPrimeInvest.AssetJobRunner.Functions
{
    public class SyncFundDailyPricesFunction
    {
        private readonly ILogger _logger;
        private readonly IFundService _fundService;
        private readonly IFundDailyPriceService _fundDailyPriceService;

        public SyncFundDailyPricesFunction(
            ILoggerFactory loggerFactory,
            IFundService fundService,
            IFundDailyPriceService fundDailyPriceService)
        {
            _logger = loggerFactory.CreateLogger<SyncFundDailyPricesFunction>();
            _fundService = fundService;
            _fundDailyPriceService = fundDailyPriceService;
        }

        [Function("SyncFundDailyPrices")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "sync-fund-daily-prices/{code}")] HttpRequestData req,
            string code)
        {
            _logger.LogInformation($"Starting sync for fund: {code}");

            try
            {
                // Buscar o fundo pelo código
                var fund = await _fundService.GetByCodeAsync(code);
                if (fund == null)
                {
                    _logger.LogWarning($"Fund not found for code: {code}");
                    var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                    await notFoundResponse.WriteStringAsync($"Fund with code '{code}' not found.");
                    return notFoundResponse;
                }

                // Fazer request para o StatusInvest
                var dailyPrices = await FetchFundDailyPricesFromStatusInvest(code);
                if (dailyPrices == null || !dailyPrices.Any())
                {
                    _logger.LogWarning($"No data returned from StatusInvest for fund: {code}");
                    var noDataResponse = req.CreateResponse(System.Net.HttpStatusCode.NoContent);
                    await noDataResponse.WriteStringAsync($"No data found for fund '{code}' from StatusInvest.");
                    return noDataResponse;
                }

                // Preencher dias faltantes
                var filledDailyPrices = FillMissingDays(dailyPrices);

                // Buscar registros existentes no banco
                var filter = new FilterFundDailyPrice { FundId = fund.Id };
                List<FundDailyPriceDto>? existingRecords = await _fundDailyPriceService.GetAllAsync(filter);

                // Separar novos e existentes
                var createDtos = new List<CreateFundDailyPriceDto>();
                var updateDtos = new List<UpdateFundDailyPriceDto>();

                foreach (var dailyPrice in filledDailyPrices)
                {
                    var existing = existingRecords.FirstOrDefault(r => r.Date == dailyPrice.Date);

                    if (existing != null)
                    {
                        // Atualizar registro existente
                        updateDtos.Add(new UpdateFundDailyPriceDto
                        {
                            Id = existing.Id,
                            Name = existing.Name,
                            Code = existing.Code,
                            Type = existing.Type,
                            OpenPrice = dailyPrice.OpenPrice,
                            ClosePrice = dailyPrice.ClosePrice,
                            MaxPrice = dailyPrice.MaxPrice,
                            MinPrice = dailyPrice.MinPrice
                        });
                    }
                    else
                    {
                        // Criar novo registro
                        createDtos.Add(new CreateFundDailyPriceDto
                        {
                            Date = dailyPrice.Date,
                            OpenPrice = dailyPrice.OpenPrice,
                            ClosePrice = dailyPrice.ClosePrice,
                            MaxPrice = dailyPrice.MaxPrice,
                            MinPrice = dailyPrice.MinPrice,
                            FundId = fund.Id
                        });
                    }
                }

                _logger.LogInformation($"Salvando novos dados.");

                // Persistir alterações
                if (createDtos.Count > 0)
                {
                    await _fundDailyPriceService.CreateManyAsync(createDtos);
                    _logger.LogInformation($"{createDtos.Count} new records created for fund {code}.");
                }

                _logger.LogInformation($"Atualizando dados.");

                if (updateDtos.Count > 0)
                {
                    await _fundDailyPriceService.UpdateManyAsync(updateDtos);
                    _logger.LogInformation($"{updateDtos.Count} records updated for fund {code}.");
                }

                _logger.LogWarning($"Atualizando preços FundCode = {fund.Code}.");
                var updateFundDto = new UpdateFundDto
                {
                    Id = fund.Id,
                    Code = fund.Code,
                    Name = fund.Name,
                    Type = fund.Type.ToString(),
                    Price = filledDailyPrices.Last().ClosePrice,
                    MinPrice = filledDailyPrices.Min(d => d.MinPrice),
                    MaxPrice = filledDailyPrices.Max(d => d.MaxPrice),
                };

                await _fundService.UpdateAsync(fund.Id, updateFundDto);
                _logger.LogInformation($"Preços do fundo {fund.Code} atualizado.");

                var successResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await successResponse.WriteStringAsync($"Fund daily prices for '{code}' synced successfully. Created: {createDtos.Count}, Updated: {updateDtos.Count}");

                return successResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error syncing fund daily prices for {code}: {ex}", ex);
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync($"Error: {ex.Message}");
                return errorResponse;
            }
        }

        private async Task<List<CreateFundDailyPriceDto>?> FetchFundDailyPricesFromStatusInvest(string code)
        {
            _logger.LogInformation($"Fetching daily prices for fund: {code}");
            using var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://statusinvest.com.br/fii/tickerprice")
            {
                Content = new StringContent($"ticker={code}&type=4&currences%5B%5D=1")
            };

            request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            request.Headers.Add("Referer", $"https://statusinvest.com.br/fundos-imobiliarios/{code.ToLower()}");
            request.Headers.Add("sec-ch-ua", "\"Google Chrome\";v=\"131\", \"Chromium\";v=\"131\", \"Not_A Brand\";v=\"24\"");
            request.Headers.Add("sec-ch-ua-mobile", "?0");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36");
            request.Headers.Add("Accept", "*/*");

            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Response from StatusInvest: {json}");
            var data = JsonSerializer.Deserialize<List<StatusInvestPriceDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            });

            return data?.SelectMany(d => d.Prices.Select(p => new CreateFundDailyPriceDto
            {
                Date = DateTime.ParseExact(p.Date, "dd/MM/yy HH:mm", CultureInfo.InvariantCulture),
                OpenPrice = p.Price,
                ClosePrice = p.Price,
                MaxPrice = p.Price,
                MinPrice = p.Price
            })).ToList();
        }

        private List<CreateFundDailyPriceDto> FillMissingDays(List<CreateFundDailyPriceDto> dailyPrices)
        {
            if (dailyPrices == null || !dailyPrices.Any()) return new List<CreateFundDailyPriceDto>();

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

            // Adicionar o último preço
            filledPrices.Add(orderedPrices.Last());

            return filledPrices;
        }
    }
}
