using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundReport;

namespace PrismaPrimeInvest.Application.Validations.FundReportValidations;

public class CreateFundReportValidator : AbstractValidator<CreateFundReportDto>
{
    public CreateFundReportValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("O campo Type é obrigatório.");

        RuleFor(x => x.FundId)
            .NotEmpty().WithMessage("O campo FundId é obrigatório.");

        RuleFor(x => x.ReferenceDate)
            .NotEmpty().WithMessage("O campo ReferenceDate deve ser informado.");
    }
}
