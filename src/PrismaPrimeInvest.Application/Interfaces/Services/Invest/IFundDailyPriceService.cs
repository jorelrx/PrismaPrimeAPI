using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Invest;

public interface IFundDailyPriceService : IBaseService<FundDailyPriceDto, CreateFundDailyPriceDto, UpdateFundDailyPriceDto, FilterFundDailyPrice> 
{
    Task SyncFundDailyPrices(Guid fundId, List<CreateFundDailyPriceDto> dailyPrices);
}
