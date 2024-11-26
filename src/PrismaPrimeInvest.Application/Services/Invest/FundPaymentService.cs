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
) : BaseService<
    FundPayment, 
    FundPaymentDto, 
    CreateFundPaymentDto, 
    UpdateFundPaymentDto, 
    CreateValidationFundPayment, 
    UpdateValidationFundPayment, 
    FilterFundPayment
>(repository, mapper), IFundPaymentService 
{
    protected override IQueryable<FundPayment> ApplyFilters(IQueryable<FundPayment> query, FilterFundPayment filter)
    {
        query = base.ApplyFilters(query, filter);

        if (filter.FundId != null)
            query = query.Where(x => x.FundId == filter.FundId);

        query = query.OrderBy(x => x.PaymentDate);

        return query;
    }
}