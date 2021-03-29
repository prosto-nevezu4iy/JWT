using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Application.Common.Interfaces;
using Domain.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public EmailService(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public async Task SendAsync(string to, string subject, string html, string from = null)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? Options.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(Options.SmtpHost, Options.SmtpPort, false).ConfigureAwait(false);
                await smtp.AuthenticateAsync(Options.SmtpUser, Options.SmtpPass).ConfigureAwait(false);
                await smtp.SendAsync(email).ConfigureAwait(false);
                await smtp.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public async Task SendVerificationEmailAsync(User entity)
        {
            var message = $@"<p>Please use the below token to verify your email address with the <code>/api/users/verify-email</code> api route:</p>
                             <p><code>{entity.VerificationToken}</code></p>";

            await SendAsync(
                to: entity.Email,
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );
        }

        public async Task SendPasswordResetEmailAsync(User entity)
        {
            var message = $@"<p>Please use the below token to reset your password with the <code>/api/users/reset-password</code> api route:</p>
                             <p><code>{entity.ResetToken}</code></p>";

            await SendAsync(
                to: entity.Email,
                subject: "Sign-up Verification API - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }
    }
}
