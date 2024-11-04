using AutoMapper;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;
using PrismaPrimeInvest.Application.Filters;
using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Validations.FundPaymentValidations;
using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;

namespace PrismaPrimeInvest.Application.Services.Invest;

public class FundPaymentService(
    IFundPaymentRepository repository,
    IMapper mapper
) : BaseService<FundPayment, FundPaymentDto, CreateFundPaymentDto, UpdateFundPaymentDto, CreateValidationFundPayment, UpdateValidationFundPayment, FilterFundPayment>(repository, mapper), IFundPaymentService {}