using FluentValidation;

namespace Application.Users.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
