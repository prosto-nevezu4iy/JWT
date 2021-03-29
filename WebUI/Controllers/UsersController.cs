using Application.Users.Commands.Authenticate;
using Application.Users.Commands.ForgotPassword;
using Application.Users.Commands.RefreshToken;
using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.ResetPassword;
using Application.Users.Commands.RevokeToken;
using Application.Users.Commands.VerifyEmail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class UsersController : ApiControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            await Mediator.Send(command);

            return Ok(new { message = "Registration successful, please check your email for verification instructions" });
        }

        [HttpPost("Verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailCommand command)
        {
            await Mediator.Send(command);

            return Ok(new { message = "Verification successful, you can now login" });
        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateCommand command)
        {
            var response = await Mediator.Send(command);

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("Forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            await Mediator.Send(command);

            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [HttpPost("Reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await Mediator.Send(command);

            return Ok(new { message = "Password reset successful, you can now login" });
        }

        [HttpPost("Refresh-token")]
        public async Task<ActionResult<AuthenticateResponse>> RefreshToken(RefreshTokenCommand command)
        {
            command.Token = Request.Cookies["refreshToken"];

            var response = await Mediator.Send(command);

            SetTokenCookie(command.Token);

            return Ok(response);
        }

        [HttpPost("Revoke-token")]
        public async Task<ActionResult<AuthenticateResponse>> RevokeToken(RevokeTokenCommand command)
        {
            // accept token from request body or cookie
            command.Token = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(command.Token))
                return BadRequest(new { message = "Token is required" });

            await Mediator.Send(command);

            return Ok(new { message = "Token revoked" });
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
