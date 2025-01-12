using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.WalletUserDTOs;

namespace PrismaPrimeInvest.Application.Validations.WalletUserValidations;

public class CreateValidationWalletUser : BaseValidation<CreateWalletUserDto>
{
    public CreateValidationWalletUser()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.WalletId)
            .NotEmpty().WithMessage("WalletId is required.");
    }
}
