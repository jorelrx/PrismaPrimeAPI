using PrismaPrimeInvest.Application.DTOs.InvestDTOs;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Invest;

public interface IFundService : IBaseService<FundDto, CreateFundDto, UpdateFundDto, FilterFund> {}