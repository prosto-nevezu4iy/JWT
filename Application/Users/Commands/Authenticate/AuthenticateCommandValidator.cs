using FluentValidation;

namespace Application.Users.Commands.Authenticate
{
    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(u => u.Password)
                .NotEmpty();
        }
    }
}
