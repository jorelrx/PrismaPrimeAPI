using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.UserDTOs;

namespace PrismaPrimeInvest.Application.Validations.UserValidations;

public class UpdateValidationUser : BaseValidation<UpdateUserDto>
{
    public UpdateValidationUser()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("Document is required.");
    }
}
