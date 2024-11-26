using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.AssetJobRunner.Models;

namespace PrismaPrimeInvest.AssetJobRunner.Services;

public class FundPaymentSyncService(IFundPaymentService fundPaymentService, IFundDailyPriceService dailyPriceService)
{
    private readonly IFundPaymentService _fundPaymentService = fundPaymentService;
    private readonly IFundDailyPriceService _dailyPriceService = dailyPriceService;

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

        await _fundPaymentService.CreateManyAsync(fundPayments);

    }
}
