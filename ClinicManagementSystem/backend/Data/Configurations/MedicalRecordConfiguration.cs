using ClinicManagementSystem.backend.Data.Configurations.Base;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Data.Configurations
{
    /// <summary>
    /// Configures the MedicalRecord entity.
    /// Defines database mapping, relationships,
    /// indexes, and default values.
    /// </summary>
    public class MedicalRecordConfiguration : BaseEntityConfiguration<MedicalRecord>
    {
        public override void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            base.Configure(builder);

            // ============================
            // Table Configuration
            // ============================

            builder.ToTable("MedicalRecords");

            // ============================
            // Relationships
            // ============================

            // One Medical Record
            //       │
            //       ▼
            // One Appointment
            builder.HasOne(record => record.Appointment)
                   .WithOne(appointment => appointment.MedicalRecord)
                   .HasForeignKey<MedicalRecord>(record => record.AppointmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One Medical Record
            //       │
            //       ▼
            // Many Prescriptions
            builder.HasMany(record => record.Prescriptions)
                   .WithOne(prescription => prescription.MedicalRecord)
                   .HasForeignKey(prescription => prescription.MedicalRecordId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // Indexes
            // ============================

            // One appointment can have only one medical record.
            builder.HasIndex(record => record.AppointmentId)
                   .IsUnique();

            // Improves searching by follow-up date.
            builder.HasIndex(record => record.FollowUpDate);
        }

    }
}
