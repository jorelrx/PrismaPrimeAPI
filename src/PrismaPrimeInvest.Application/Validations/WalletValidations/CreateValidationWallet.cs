using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.WalletDTOs;

namespace PrismaPrimeInvest.Application.Validations.WalletValidations;

public class CreateValidationWallet : BaseValidation<CreateWalletDto>
{
    public CreateValidationWallet()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("Document is required.");
    }
}
