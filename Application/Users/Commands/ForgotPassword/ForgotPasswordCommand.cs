using MediatR;

namespace Application.Users.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest
    {
        public string Email { get; set; }
    }
}
