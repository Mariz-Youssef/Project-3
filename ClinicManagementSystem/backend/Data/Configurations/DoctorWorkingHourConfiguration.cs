using ClinicManagementSystem.backend.Data.Configurations.Base;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Data.Configurations
{
    /// <summary>
    /// Configures the DoctorWorkingHour entity.
    /// Defines database mapping, enum conversion,
    /// indexes, relationships, and default values.
    /// </summary>
    public class DoctorWorkingHourConfiguration : BaseEntityConfiguration<DoctorWorkingHour>
    {
        public override void Configure(EntityTypeBuilder<DoctorWorkingHour> builder)
        {
            base.Configure(builder);

            // ============================
            // Table Configuration
            // ============================

            builder.ToTable("DoctorWorkingHours");

            // ============================
            // Enum Conversion
            // ============================

            // Store DayOfWeek as text instead of integer.
            builder.Property(workingHour => workingHour.DayOfWeek)
                   .HasConversion<string>();

            // ============================
            // Relationships
            // ============================

            // One Doctor
            //      │
            //      ▼
            // Many Working Hours
            builder.HasOne(workingHour => workingHour.Doctor)
                   .WithMany(doctor => doctor.WorkingHours)
                   .HasForeignKey(workingHour => workingHour.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // Indexes
            // ============================

            // Improves searching schedules by doctor.
            builder.HasIndex(workingHour => workingHour.DoctorId);

            // Prevent duplicate schedules
            // for the same doctor and day.
            builder.HasIndex(workingHour => new
            {
                workingHour.DoctorId,
                workingHour.DayOfWeek
            })
            .IsUnique();
        }
    }
}
