using Application.Users.Commands.Authenticate;
using MediatR;

namespace Application.Users.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthenticateResponse>
    {
        public string Token { get; set; }
    }
}
