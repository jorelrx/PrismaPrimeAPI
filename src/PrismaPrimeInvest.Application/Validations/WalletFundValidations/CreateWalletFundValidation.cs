using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.WalletFundDTOs;

namespace PrismaPrimeInvest.Application.Validations.WalletFundValidations
{
    public class CreateWalletFundValidation : AbstractValidator<CreateWalletFundDto>
    {
        public CreateWalletFundValidation()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty().WithMessage("UserId é obrigatório.");

            RuleFor(x => x.FundId)
                .NotEmpty().WithMessage("FundId é obrigatório.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

            RuleFor(x => x.PurchasePrice)
                .GreaterThan(0).WithMessage("O preço de compra deve ser maior que zero.");

            RuleFor(x => x.PurchaseDate)
                .NotEmpty().WithMessage("A data de compra é obrigatória.");
        }
    }
}
