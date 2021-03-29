using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;

        public ForgotPasswordCommandHandler(
            IApplicationDbContext context,
            IEmailService emailService, 
            ITokenService tokenService)
        {
            _context = context;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Email);
            };

            user.ResetToken = _tokenService.GenerateRandomToken();
            user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendPasswordResetEmailAsync(user);

            return Unit.Value;
        }
    }
}
