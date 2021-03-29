using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.RevokeToken
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
    {
        private readonly IApplicationDbContext _context;

        public RevokeTokenCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.RefreshToken == request.Token);

            user.RefreshToken = null;
            user.RefreshTokenExpires = null;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
