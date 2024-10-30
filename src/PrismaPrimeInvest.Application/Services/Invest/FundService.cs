using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Validations.FundValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;

namespace PrismaPrimeInvest.Application.Services.Invest;

public class FundService(
    IFundRepository repository,
    IMapper mapper
) : BaseService<Fund, FundDto, CreateFundDto, UpdateFundDto, CreateValidationFund, UpdateValidationFund, FilterFund>(repository, mapper), IFundService {}