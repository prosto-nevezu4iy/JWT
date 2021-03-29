using Application.Common.Security;
using Domain.Enums;
using MediatR;

namespace Application.Users.Commands.RevokeToken
{
    [Authorize(Role.User, Role.Company)]
    public class RevokeTokenCommand : IRequest
    {
        public string Token { get; set; }
    }
}
