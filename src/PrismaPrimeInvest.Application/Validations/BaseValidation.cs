using FluentValidation;

namespace PrismaPrimeInvest.Application.Validations
{
    public abstract class BaseValidation<T> : AbstractValidator<T>
    {
        protected BaseValidation()
        {
        }
    }
}
