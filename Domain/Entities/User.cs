using Domain.Common;
using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue;
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
    }
}
