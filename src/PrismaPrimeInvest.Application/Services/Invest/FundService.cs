using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Validations.FundValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;

namespace PrismaPrimeInvest.Application.Services.Invest;

public class FundService(
    IFundRepository repository,
    IFundDailyPriceService fundDailyPriceService,
    IFundPaymentService fundPaymentService,
    IMapper mapper
) : BaseService<Fund, FundDto, CreateFundDto, UpdateFundDto, CreateValidationFund, UpdateValidationFund, FilterFund>(repository, mapper), IFundService 
{
    private readonly IFundDailyPriceService _fundDailyPriceService = fundDailyPriceService;
    private readonly IFundPaymentService _fundPaymentService = fundPaymentService;

    protected override IQueryable<Fund> ApplyFilters(IQueryable<Fund> query, FilterFund filter)
    {
        if (!string.IsNullOrEmpty(filter.Code))
            query = query.Where(f => f.Code == filter.Code);

        return query;
    }

    public async Task<Fund?> GetByCodeAsync(string code)
    {
        var query = _repository.GetAllAsync();
        return await query.FirstOrDefaultAsync(f => f.Code == code);
    }

    public async Task<List<Fund>> GetAllEntitiesAsync(FilterFund filter)
    {
        var query = _repository.GetAllAsync();
        query = ApplyFilters(query, filter);
        var entities = await query.Include(p => p.DailyPrices).ToListAsync();
        return entities;
    }

    public async Task<List<MonthlyInvestmentReport>> AnalyzeInvestment(string ticker, int PurchaseDay, double BaseAmount)
    {
        var fund = await GetByCodeAsync(ticker) ?? throw new Exception("Fundo não encontrado");

        var dailyPrices = await _fundDailyPriceService.GetAllAsync(new()
        {
            FundId = fund.Id
        });

        var payments = await _fundPaymentService.GetAllAsync(new()
        {
            FundId = fund.Id
        });

        var monthlyPrices = dailyPrices
            .GroupBy(p => new { p.Date.Year, p.Date.Month })
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(p => p.Date).ToList()
            );

        var monthlyReport = new List<MonthlyInvestmentReport>();
        double totalInvestmentWithoutDividends = 0;
        double totalDividends = 0;
        double totalShares = 0;
        double residualAmount = 0;

        foreach (var monthGroup in monthlyPrices)
        {
            var year = monthGroup.Key.Year;
            var month = monthGroup.Key.Month;
            var prices = monthGroup.Value;

            var purchasePrice = prices
                .OrderBy(p => Math.Abs(p.Date.Day - PurchaseDay))
                .FirstOrDefault();

            if (purchasePrice != null)
            {
                double availableAmount = BaseAmount + residualAmount;

                var sharesToBuy = Math.Floor(availableAmount / purchasePrice.ClosePrice);
                var investmentForMonth = sharesToBuy * purchasePrice.ClosePrice;

                residualAmount = availableAmount - investmentForMonth;

                totalShares += sharesToBuy;
                totalInvestmentWithoutDividends += BaseAmount;

                var monthlyDividend = payments
                    .Where(p => p.PaymentDate.Year == year && p.PaymentDate.Month == month)
                    .Sum(p => p.Dividend * totalShares);

                totalDividends += monthlyDividend;
                residualAmount += monthlyDividend;

                var lastPriceOfMonth = prices.LastOrDefault()?.ClosePrice ?? 0;
                var currentPortfolioValue = totalShares * lastPriceOfMonth;

                monthlyReport.Add(new MonthlyInvestmentReport
                {
                    Date = $"{month:D2}/{year}",
                    TotalInvestmentWithoutDividends = totalInvestmentWithoutDividends,
                    TotalInvestmentWithDividends = totalInvestmentWithoutDividends + totalDividends,
                    FundPriceOnPurchaseDay = purchasePrice.ClosePrice,
                    SharesPurchasedThisMonth = sharesToBuy,
                    TotalSharesPurchased = totalShares,
                    TotalAvailableForInvestment = availableAmount,
                    DividendsThisMonth = monthlyDividend,
                    TotalDividends = totalDividends,
                    PortfolioValueAtEndOfMonth = currentPortfolioValue
                });
            }
        }

        return monthlyReport;
    }
}
