using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;

namespace PrismaPrimeInvest.Application.Validations.FundDailyPriceValidations;

public class CreateValidationFundDailyPrice : BaseValidation<CreateFundDailyPriceDto>
{
    public CreateValidationFundDailyPrice()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required.");

        RuleFor(x => x.MaxPrice)
            .NotEmpty().WithMessage("MaxPrice is required.");

        RuleFor(x => x.MinPrice)
            .NotEmpty().WithMessage("MinPrice is required.");

        RuleFor(x => x.FundId)
            .NotEmpty().WithMessage("FundId is required.");
    }
}
