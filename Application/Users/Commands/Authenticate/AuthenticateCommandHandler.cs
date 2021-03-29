using Application.Common.Exceptions;
using Application.Common.Interfaces;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Application.Users.Commands.Authenticate
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthenticateCommandHandler(IApplicationDbContext context,
            ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !user.IsVerified || !BC.Verify(request.Password, user.PasswordHash))
                throw new ValidationException(new ValidationFailure("Email", "Email or password is incorrect"));

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
