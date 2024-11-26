using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;

namespace PrismaPrimeInvest.Application.Validations.FundPaymentValidations;

public class UpdateValidationFundPayment : BaseValidation<UpdateFundPaymentDto>
{
    public UpdateValidationFundPayment()
    {
        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required.");

        RuleFor(x => x.Dividend)
            .NotEmpty().WithMessage("Dividend is required.");

        RuleFor(x => x.MinimumPrice)
            .NotEmpty().WithMessage("MinimumPrice is required.");
            
        RuleFor(x => x.MaximumPrice)
            .NotEmpty().WithMessage("MaximumPrice is required.");
    }
}
