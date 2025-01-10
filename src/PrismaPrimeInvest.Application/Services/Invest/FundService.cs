using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Interfaces.Services.Utilities;
using PrismaPrimeInvest.Application.Services.Utilities;
using PrismaPrimeInvest.Application.Validations.FundValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Enums;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;

namespace PrismaPrimeInvest.Application.Services.Invest;

public class FundService(
    IFundRepository repository,
    IFundDailyPriceService fundDailyPriceService,
    IFundPaymentService fundPaymentService,
    IAssetReportDownloader assetReportDownloader,
    AssetHttpService assetHttpService,
    IMapper mapper
) : BaseService<Fund, FundDto, CreateFundDto, UpdateFundDto, CreateValidationFund, UpdateValidationFund, FilterFund>(repository, mapper), IFundService 
{
    private readonly IFundDailyPriceService _fundDailyPriceService = fundDailyPriceService;
    private readonly IFundPaymentService _fundPaymentService = fundPaymentService;
    private readonly IAssetReportDownloader _assetReportDownloader = assetReportDownloader;
    private readonly AssetHttpService _assetHttpService = assetHttpService;

    protected override IQueryable<Fund> ApplyFilters(IQueryable<Fund> query, FilterFund filter)
    {
        if (!string.IsNullOrEmpty(filter.Code))
            query = query.Where(f => f.Code == filter.Code);

        return query;
    }

    public override async Task<Guid> CreateAsync(CreateFundDto dto)
    {
        await _createValidator.ValidateAndThrowAsync(dto);
        
        string? idReport = await _assetReportDownloader.GetIdReportByCnpjAsync(dto.Cnpj) ?? 
            throw new Exception("Não foi possível encontrar o ID do relatório");

        string? xmlData = await _assetReportDownloader.DownloadXmlAsync(idReport, dto.Cnpj) ??
            throw new Exception("Não foi possível buscar relátorio Xml");

        AssetData? assetData = _assetReportDownloader.DeserializeXml(xmlData) ??
            throw new Exception("Não foi possível descerializar objeto AssetData");

        Fund fund = new()
        {
            Cnpj = dto.Cnpj,
            Code = dto.Ticker,
            Name = assetData.GeneralData.FundName,
            Type = Enum.Parse<FundTypeEnum>(dto.Type),
            QtyQuotasIssued = assetData.GeneralData.IssuedSharesQuantity,
            NetAssetValue = assetData.MonthlyReport.Summary.NetWorth,
            TotalShares = assetData.MonthlyReport.Shareholders.Total,
            NetAssetValuePerShare = assetData.MonthlyReport.Summary.ShareValue,
            Price = assetData.MonthlyReport.Summary.ShareValue,
            MinPrice = assetData.MonthlyReport.Summary.ShareValue,
            MaxPrice = assetData.MonthlyReport.Summary.ShareValue
        };

        await _repository.CreateAsync(fund);

        var fundEntity = await GetByCodeAsync(dto.Ticker) ??
            throw new Exception("Erro ao buscar entidade adicionada!");

        int assetType = fund.Type == FundTypeEnum.Fiagro ? 1 : 2;
        IEnumerable<DailyPriceResponse>? dailyPrices = await _assetHttpService.GetDailyPricesByTickerAsync(dto.Ticker, assetType) ??
            throw new Exception("Falha ao buscar cotações diárias");

        List<CreateFundDailyPriceDto>? createFundDailyPriceDtos = dailyPrices?.SelectMany(d => d.Prices.Select(p => new CreateFundDailyPriceDto
        {
            Date = p.DateConterted,
            OpenPrice = p.Price,
            ClosePrice = p.Price,
            MaxPrice = p.Price,
            MinPrice = p.Price
        })).ToList();

        if (createFundDailyPriceDtos != null)
        {
            var filledDailyPrices = FillMissingDays(createFundDailyPriceDtos);

            await _fundDailyPriceService.SyncFundDailyPrices(fund.Id, filledDailyPrices);

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

            await UpdateAsync(fund.Id, updateFundDto);
        }

        EarningsResponse? earningsResponse = await _assetHttpService.GetEarningsAsync(dto.Ticker, assetType);
        if (earningsResponse != null)
        {
            await _fundPaymentService.SyncFundPayments(fund.Id, earningsResponse);
        }

        return fundEntity.Id;
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
