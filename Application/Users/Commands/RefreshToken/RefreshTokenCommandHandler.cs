using Application.Common.Interfaces;
using Application.Users.Commands.Authenticate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticateResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(
            IApplicationDbContext context, 
            ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthenticateResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.RefreshToken == request.Token);

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRandomToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpires = DateTime.Now.AddDays(7);

            await _context.SaveChangesAsync(cancellationToken);

            var response = new AuthenticateResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsVerified = user.IsVerified,
                JwtToken = accessToken,
                RefreshToken = refreshToken
            };

            return response;
        }
    }
}
