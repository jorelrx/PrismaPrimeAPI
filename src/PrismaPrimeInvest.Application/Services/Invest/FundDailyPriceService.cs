using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Validations.FundDailyPriceValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;

namespace PrismaPrimeInvest.Application.Services.Invest;

public class FundDailyPriceService(
    IFundDailyPriceRepository repository,
    IMapper mapper
) : BaseService<FundDailyPrice, FundDailyPriceDto, CreateFundDailyPriceDto, UpdateFundDailyPriceDto, CreateValidationFundDailyPrice, UpdateValidationFundDailyPrice, FilterFundDailyPrice>(repository, mapper), IFundDailyPriceService {}