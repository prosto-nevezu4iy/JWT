using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IApplicationDbContext _context;

        public RegisterCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(u => u.Email)
                .EmailAddress()
                .MaximumLength(255)
                .MustAsync(BeUniqueEmail).WithMessage("The specified email already exists.");

            RuleFor(u => u.Password)
                .NotEmpty()
                .Length(6, 25)
                .Equal(u => u.ConfirmPassword);
        }

        public async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await _context.Users.AllAsync(u => u.Email != email);
        }
    }
}
