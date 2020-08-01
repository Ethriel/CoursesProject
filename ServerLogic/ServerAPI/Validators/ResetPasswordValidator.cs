using FluentValidation;
using ServicesAPI.DataPresentation.AccountManagement;

namespace ServerAPI.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordData>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .Matches(@"(?=.*[a-z])")
                .WithMessage("Password must have at least one lower case letter")
                .Matches(@"(?=.*[A-Z])")
                .WithMessage("Password must have at least one upper case letter")
                .Matches(@"(?=.*\d)")
                .WithMessage("Password must have at least one number")
                .Matches(@"(?=.*[@$!%*?&])")
                .WithMessage("Password must have at least one special character: @ $ ! % * ? &")
                .Matches(@"[A-Za-z\d@$!%*?&]{8,}")
                .WithMessage("Password must be at least eight characters long");
        }
    }
}
