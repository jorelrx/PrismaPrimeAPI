using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Domain.Entities.Invest;

namespace PrismaPrimeInvest.AssetJobRunner.Functions
{
    public class AnalyzeHistoricalBestBuyFunction
    {
        private readonly ILogger _logger;
        private readonly IFundService _fundService;
        private readonly IMapper _mapper;

        public AnalyzeHistoricalBestBuyFunction(ILoggerFactory loggerFactory, IFundService fundService, IMapper mapper)
        {
            _logger = loggerFactory.CreateLogger<AnalyzeHistoricalBestBuyFunction>();
            _fundService = fundService;
            _mapper = mapper;
        }

        [Function("AnalyzeHistoricalBestBuy")]
        public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"AnalyzeHistoricalBestBuy function executed at: {DateTime.Now}");

            var funds = await _fundService.GetAllEntitiesAsync(new());

            foreach (var fund in funds)
            {
                if (fund.DailyPrices == null || !fund.DailyPrices.Any())
                {
                    _logger.LogWarning($"Fund {fund.Name} ({fund.Code}) has no historical daily prices.");
                    continue;
                }

                var bestDayAnalysis = AnalyzeBestBuyDay(fund.DailyPrices);

                if (bestDayAnalysis != null)
                {
                    fund.BestBuyDay = bestDayAnalysis.Day;
                    fund.BestBuyDayPrice = bestDayAnalysis.AveragePrice;

                    _logger.LogInformation($"Best buy day for {fund.Name} ({fund.Code}): Day {fund.BestBuyDay}, Avg Price {fund.BestBuyDayPrice}");

                    await _fundService.UpdateAsync(fund.Id, _mapper.Map<UpdateFundDto>(fund));
                }
            }
        }

        private BestBuyDayAnalysis? AnalyzeBestBuyDay(IEnumerable<FundDailyPrice> dailyPrices)
        {
            var groupedByDay = dailyPrices
                .GroupBy(dp => dp.Date.Day)
                .Select(g => new
                {
                    Day = g.Key,
                    AveragePrice = g.Average(dp => dp.ClosePrice),
                    MinPrice = g.Min(dp => dp.ClosePrice),
                    Occurrences = g.Count()
                })
                .OrderBy(g => g.AveragePrice) // Ordenar pelos menores preços médios
                .ThenBy(g => g.Occurrences) // Desempatar com base na frequência
                .ToList();

            if (!groupedByDay.Any()) return null;

            // Retornar o melhor dia baseado na menor média e mais frequente
            var bestDay = groupedByDay.First();
            return new BestBuyDayAnalysis
            {
                Day = bestDay.Day,
                AveragePrice = bestDay.AveragePrice
            };
        }
    }

    public class BestBuyDayAnalysis
    {
        public int Day { get; set; }
        public double AveragePrice { get; set; }
    }
}
