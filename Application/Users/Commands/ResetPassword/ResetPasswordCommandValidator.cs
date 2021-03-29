using FluentValidation;

namespace Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(u => u.Token)
                .NotEmpty();

            RuleFor(u => u.Password)
                 .NotEmpty()
                 .Length(6, 25)
                 .Equal(u => u.ConfirmPassword);
        }
    }
}
