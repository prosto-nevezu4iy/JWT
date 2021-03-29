using Domain.Enums;
using MediatR;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterCommand : IRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
