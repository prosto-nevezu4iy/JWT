using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler: IRequestHandler<VerifyEmailCommand>
    {
        private readonly IApplicationDbContext _context;

        public VerifyEmailCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.VerificationToken == request.Token);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Token);
            }

            user.Verified = DateTime.UtcNow;
            user.VerificationToken = null;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
