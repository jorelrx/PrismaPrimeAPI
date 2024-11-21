using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Domain.Entities.Invest;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Invest;

public interface IFundService : IBaseService<FundDto, CreateFundDto, UpdateFundDto, FilterFund> 
{
    Task<List<Fund>> GetAllEntitiesAsync(FilterFund filter);
    Task<Fund?> GetByCodeAsync(string code);
}