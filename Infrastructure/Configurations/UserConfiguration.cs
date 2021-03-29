using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(t => t.UserName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(t => t.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(t => t.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(t => t.VerificationToken)
               .HasMaxLength(255);

            builder.Property(t => t.ResetToken)
                .HasMaxLength(255);

            builder.Property(t => t.RefreshToken)
                .HasMaxLength(255);
        }
    }
}
