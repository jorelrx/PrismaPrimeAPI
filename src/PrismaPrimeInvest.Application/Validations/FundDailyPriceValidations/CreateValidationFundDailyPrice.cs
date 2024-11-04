using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;

namespace PrismaPrimeInvest.Application.Validations.FundDailyPriceValidations;

public class CreateValidationFundDailyPrice : BaseValidation<CreateFundDailyPriceDto>
{
    public CreateValidationFundDailyPrice()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.");
    }
}
