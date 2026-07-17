using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Common.Data.Configurations
{
    /// <summary>
    /// Configures the ApplicationUser entity.
    /// Defines custom property constraints and one-to-one relationships
    /// with Doctor and Patient.
    /// </summary>
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // ============================
            // Table Configuration
            // ============================

            builder.ToTable("Users");

            // ============================
            // Property Configurations
            // ============================

            builder.Property(user => user.FullName)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(user => user.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(user => user.IsDeleted)
                   .HasDefaultValue(false);

            // ============================
            // Relationships
            // ============================

            /// One ApplicationUser
            ///        ↓
            ///      One Doctor
            builder.HasOne(user => user.Doctor)
                   .WithOne(doctor => doctor.User)
                   .HasForeignKey<Doctor>(doctor => doctor.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            /// One ApplicationUser
            ///        ↓
            ///      One Patient
            builder.HasOne(user => user.Patient)
                   .WithOne(patient => patient.User)
                   .HasForeignKey<Patient>(patient => patient.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // Indexes
            // ============================

            builder.HasIndex(user => user.FullName);
        }
    }
}
