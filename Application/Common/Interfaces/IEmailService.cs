using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string html, string from = null);
        Task SendVerificationEmailAsync(User entity);
        Task SendPasswordResetEmailAsync(User entity);
    }
}
