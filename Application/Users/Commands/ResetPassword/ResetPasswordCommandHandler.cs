using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IApplicationDbContext _context;

        public ResetPasswordCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x =>
                x.ResetToken == request.Token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Token);
            }

            user.PasswordHash = BC.HashPassword(request.Password);
            user.ResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
