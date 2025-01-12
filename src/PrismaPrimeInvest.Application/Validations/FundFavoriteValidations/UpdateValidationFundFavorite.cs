using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.FundFavoriteDTOs;

namespace PrismaPrimeInvest.Application.Validations.FundFavoriteValidations;

public class UpdateValidationFundFavorite : BaseValidation<UpdateFundFavoriteDto>
{
    public UpdateValidationFundFavorite()
    {
        RuleFor(x => x.FundId)
            .NotEmpty().WithMessage("FundId is required.");
    }
}
