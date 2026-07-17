using ClinicManagementSystem.backend.Common.Data.Configurations.Base;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Common.Data.Configurations
{
    /// <summary>
    /// Configures the DoctorLeave entity.
    /// Defines database mapping, relationships,
    /// indexes, and default values.
    /// </summary>
    public class DoctorLeaveConfiguration : BaseEntityConfiguration<DoctorLeave>
    {
        public override void Configure(EntityTypeBuilder<DoctorLeave> builder)
        {
            base.Configure(builder);

            // ============================
            // Table Configuration
            // ============================

            builder.ToTable("DoctorLeaves");

            // ============================
            // Relationships
            // ============================

            // One Doctor
            //      │
            //      ▼
            // Many Leave Records
            builder.HasOne(leave => leave.Doctor)
                   .WithMany(doctor => doctor.Leaves)
                   .HasForeignKey(leave => leave.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // Indexes
            // ============================

            // Improves searching leave records by doctor.
            builder.HasIndex(leave => leave.DoctorId);

            // Improves searching leave records by date range.
            builder.HasIndex(leave => new
            {
                leave.StartDate,
                leave.EndDate
            });
        }
    }
}
