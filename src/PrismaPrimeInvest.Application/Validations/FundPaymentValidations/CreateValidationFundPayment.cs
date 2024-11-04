using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;

namespace PrismaPrimeInvest.Application.Validations.FundPaymentValidations;

public class CreateValidationFundPayment : BaseValidation<CreateFundPaymentDto>
{
    public CreateValidationFundPayment()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.");
    }
}
