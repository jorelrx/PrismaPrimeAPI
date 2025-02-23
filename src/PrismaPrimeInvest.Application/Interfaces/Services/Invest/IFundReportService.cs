using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundReport;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Invest;

public interface IFundReportService : IBaseService<FundReportDto, CreateFundReportDto, UpdateFundReportDto, FilterFundReport> 
{
    Task SyncByFundIdAsync(Guid fundId);
}
