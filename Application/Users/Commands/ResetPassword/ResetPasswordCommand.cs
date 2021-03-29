﻿using MediatR;

namespace Application.Users.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest
    {
        public string Token { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
