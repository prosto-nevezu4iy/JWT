using FluentValidation;

namespace Application.Users.Commands.VerifyEmail
{
    public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(u => u.Token)
                .NotEmpty();
        }
    }
}
