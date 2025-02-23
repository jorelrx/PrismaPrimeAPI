using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundReport;

namespace PrismaPrimeInvest.Application.Validations.FundReportValidations;

public class UpdateFundReportValidator : AbstractValidator<UpdateFundReportDto>
{
    public UpdateFundReportValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("O campo Status é obrigatório.");
    }
}
