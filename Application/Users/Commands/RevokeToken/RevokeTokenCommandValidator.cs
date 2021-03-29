using FluentValidation;

namespace Application.Users.Commands.RevokeToken
{
    public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenCommandValidator()
        {
            RuleFor(u => u.Token)
                .NotEmpty();
        }
    }
}
