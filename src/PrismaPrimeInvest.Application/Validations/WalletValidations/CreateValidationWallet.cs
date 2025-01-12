using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;

namespace PrismaPrimeInvest.Application.Validations.WalletValidations;

public class CreateValidationWallet : BaseValidation<CreateWalletDto>
{
    public CreateValidationWallet()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}
