using MediatR;

namespace Application.Users.Commands.Authenticate
{
    public class AuthenticateCommand : IRequest<AuthenticateResponse>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
