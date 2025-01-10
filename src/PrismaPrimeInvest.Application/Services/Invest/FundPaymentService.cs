using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;
using PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Validations.FundPaymentValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;

namespace PrismaPrimeInvest.Application.Services.Invest;

public class FundPaymentService(
    IFundPaymentRepository repository,
    IFundDailyPriceService dailyPriceService,
    IMapper mapper
) : BaseService<
    FundPayment, 
    FundPaymentDto, 
    CreateFundPaymentDto, 
    UpdateFundPaymentDto, 
    CreateValidationFundPayment, 
    UpdateValidationFundPayment, 
    FilterFundPayment
>(repository, mapper), IFundPaymentService 
{
    private readonly IFundDailyPriceService _dailyPriceService = dailyPriceService;

    protected override IQueryable<FundPayment> ApplyFilters(IQueryable<FundPayment> query, FilterFundPayment filter)
    {
        query = base.ApplyFilters(query, filter);

        if (filter.FundId != null)
            query = query.Where(x => x.FundId == filter.FundId);

        query = query.OrderBy(x => x.PaymentDate);

        return query;
    }

    public async Task SyncFundPayments(Guid fundId, EarningsResponse earningsResponse)
    {
        var filter = new FilterFundDailyPrice { FundId = fundId };
        List<FundDailyPriceDto>? fundDailyPrices = await _dailyPriceService.GetAllAsync(filter);

        List<CreateFundPaymentDto> fundPayments = [];

        foreach (var item in earningsResponse.AssetEarningsModels)
        {
            FundDailyPriceDto? fundDailyPriceDto = fundDailyPrices.FirstOrDefault(x => x.Date == Convert.ToDateTime(item.Ed));
            if (fundDailyPriceDto == null) continue;

            CreateFundPaymentDto createFundPaymentDto = new CreateFundPaymentDto
            {
                FundId = fundId,
                Dividend = Convert.ToDouble(item.V),
                Price = fundDailyPriceDto.ClosePrice,
                PaymentDate = Convert.ToDateTime(item.Ed),
                MinimumPrice = fundDailyPrices.Where(d => d.Date.Month == Convert.ToDateTime(item.Ed).Month).Min(d => d.ClosePrice),
                MaximumPrice = fundDailyPrices.Where(d => d.Date.Month == Convert.ToDateTime(item.Ed).Month).Max(d => d.ClosePrice),
            };

            fundPayments.Add(createFundPaymentDto);
        }

        await CreateManyAsync(fundPayments);
    }
}