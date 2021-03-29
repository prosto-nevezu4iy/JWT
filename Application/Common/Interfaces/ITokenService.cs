using Domain.Entities;
namespace Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateRandomToken();
        string GenerateAccessToken(User user);
    }
}
