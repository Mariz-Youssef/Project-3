using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Common.Data.Configurations
{
    /// <summary>
    /// Configures the RefreshToken entity: table mapping, constraints,
    /// indexes, and its relationship to ApplicationUser.
    /// </summary>
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(rt => rt.Token)
                   .IsUnique();

            builder.Property(rt => rt.CreatedByIp).HasMaxLength(45);
            builder.Property(rt => rt.RevokedByIp).HasMaxLength(45);
            builder.Property(rt => rt.ReplacedByToken).HasMaxLength(200);

            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rt => rt.UserId);
        }
    }
}
