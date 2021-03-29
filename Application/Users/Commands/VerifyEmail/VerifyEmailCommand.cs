﻿using MediatR;

namespace Application.Users.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest
    {
        public string Token { get; set; }
    }
}
