using FluentValidation;

namespace Application.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(u => u.Token)
                .NotEmpty();
        }
    }
}
