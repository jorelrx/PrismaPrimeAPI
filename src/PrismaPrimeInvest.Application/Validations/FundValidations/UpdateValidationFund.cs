using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs;

namespace PrismaPrimeInvest.Application.Validations.FundValidations;

public class UpdateValidationFund : BaseValidation<UpdateFundDto>
{
    public UpdateValidationFund()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.");
    }
}
