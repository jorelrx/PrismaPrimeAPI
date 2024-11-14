using FluentValidation;
using PrismaPrimeInvest.Application.DTOs.UserDTOs;

namespace PrismaPrimeInvest.Application.Validations.UserValidations;

public class CreateValidationUser : BaseValidation<CreateUserDto>
{
    public CreateValidationUser()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("Document is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
            
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .MinimumLength(6).WithMessage("UserName must be at least 6 characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
