using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;
using PrismaPrimeInvest.Application.Filters;

namespace PrismaPrimeInvest.Application.Interfaces.Services.Invest;

public interface IFundPaymentService : IBaseService<FundPaymentDto, CreateFundPaymentDto, UpdateFundPaymentDto, FilterFundPayment> {}