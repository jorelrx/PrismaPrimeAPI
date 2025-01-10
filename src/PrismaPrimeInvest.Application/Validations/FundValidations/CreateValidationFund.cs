using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;

namespace PrismaPrimeInvest.Application.Validations.FundValidations;

public class CreateValidationFund : BaseValidation<CreateFundDto>
{
    public CreateValidationFund()
    {
        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("Cnpj is required.");

        RuleFor(x => x.Ticker)
            .NotEmpty().WithMessage("Ticker is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.");
    }
}
