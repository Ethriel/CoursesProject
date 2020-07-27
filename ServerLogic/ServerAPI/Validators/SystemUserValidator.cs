using FluentValidation;
using Infrastructure.Models;

namespace ServerAPI.Validators
{
    public class SystemUserValidator : AbstractValidator<SystemUser>
    {
        public SystemUserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .NotNull();
        }
    }
}
