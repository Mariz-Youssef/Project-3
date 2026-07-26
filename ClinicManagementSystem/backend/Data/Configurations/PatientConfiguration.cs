using ClinicManagementSystem.backend.Data.Configurations.Base;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Data.Configurations
{
    /// <summary>
    /// Configures the Patient entity.
    /// Defines table mapping, property configurations,
    /// enum conversions, indexes, and relationships.
    /// </summary>
    public class PatientConfiguration : BaseEntityConfiguration<Patient>
    {
        public override void Configure(EntityTypeBuilder<Patient> builder)
        {
            base.Configure(builder);

            // ============================
            // Table Configuration
            // ============================

            builder.ToTable("Patients");

            // ============================
            // Primary Key
            // ============================

            builder.HasKey(patient => patient.Id);

            // ============================
            // Enum Conversions
            // ============================

            // Store Gender as text instead of integer.
            builder.Property(patient => patient.Gender)
                   .HasConversion<string>();

            // Store Blood Group as text instead of integer.
            builder.Property(patient => patient.BloodGroup)
                   .HasConversion<string>();

            // ============================
            // Relationships
            // ============================

            // One Patient
            //      │
            //      ▼
            // One ApplicationUser
            builder.HasOne(patient => patient.User)
                   .WithOne(user => user.Patient)
                   .HasForeignKey<Patient>(patient => patient.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One Patient
            //      │
            //      ▼
            // Many Appointments
            builder.HasMany(patient => patient.Appointments)
                   .WithOne(appointment => appointment.Patient)
                   .HasForeignKey(appointment => appointment.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // Indexes
            // ============================

            // Improves searching patients by emergency contact phone.
            builder.HasIndex(patient => patient.EmergencyContactPhone);

            // Improves filtering patients by blood group.
            builder.HasIndex(patient => patient.BloodGroup);
        }
    }
}
