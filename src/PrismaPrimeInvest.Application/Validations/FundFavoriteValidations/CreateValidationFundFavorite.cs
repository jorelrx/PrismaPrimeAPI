using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.FundFavoriteDTOs;

namespace PrismaPrimeInvest.Application.Validations.FundFavoriteValidations;

public class CreateValidationFundFavorite : BaseValidation<CreateFundFavoriteDto>
{
    public CreateValidationFundFavorite()
    {
        RuleFor(x => x.FundId)
            .NotEmpty().WithMessage("FundId is required.");
    }
}
