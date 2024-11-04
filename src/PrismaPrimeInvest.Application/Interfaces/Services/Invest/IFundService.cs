using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Invest;

public interface IFundService : IBaseService<FundDto, CreateFundDto, UpdateFundDto, FilterFund> {}