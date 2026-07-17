using ClinicManagementSystem.backend.Common.Data.Configurations.Base;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Common.Data.Configurations
{
    /// <summary>
    /// Configures the Prescription entity.
    /// Defines database mapping, relationships,
    /// indexes, and default values.
    /// </summary>
    public class PrescriptionConfiguration : BaseEntityConfiguration<Prescription>
    {
        public override void Configure(EntityTypeBuilder<Prescription> builder)
        {
            base.Configure(builder);

            // ============================
            // Table Configuration
            // ============================

            builder.ToTable("Prescriptions");


            // ============================
            // Relationships
            // ============================

            // One Medical Record
            //        │
            //        ▼
            // Many Prescriptions
            builder.HasOne(prescription => prescription.MedicalRecord)
                   .WithMany(record => record.Prescriptions)
                   .HasForeignKey(prescription => prescription.MedicalRecordId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // Indexes
            // ============================

            // Improves retrieving all prescriptions
            // belonging to a medical record.
            builder.HasIndex(prescription => prescription.MedicalRecordId);

            // Improves searching by medicine name.
            builder.HasIndex(prescription => prescription.MedicineName);
        }
    }
}
