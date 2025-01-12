using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.WalletUserDTOs;

namespace PrismaPrimeInvest.Application.Validations.WalletUserValidations;

public class UpdateValidationWalletUser : BaseValidation<UpdateWalletUserDto>
{
    public UpdateValidationWalletUser()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("First Name is required.");
    }
}
