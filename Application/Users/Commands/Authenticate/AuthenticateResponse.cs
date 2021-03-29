using System.Text.Json.Serialization;

namespace Application.Users.Commands.Authenticate
{
    public class AuthenticateResponse 
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
