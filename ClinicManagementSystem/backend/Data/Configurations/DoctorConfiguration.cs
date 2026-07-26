using ClinicManagementSystem.backend.Data.Configurations.Base;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Data.Configurations
{
    /// <summary>
    /// Configures the Doctor entity.
    /// Defines table mapping, property configurations,
    /// indexes, and relationships with other entities.
    /// </summary>
    public class DoctorConfiguration : BaseEntityConfiguration<Doctor>
    {
        public override void Configure(EntityTypeBuilder<Doctor> builder)
        {
            base.Configure(builder);

            // ============================
            // Table Configuration
            // ============================

            builder.ToTable("Doctors");

            // ============================
            // Primary Key
            // ============================

            builder.HasKey(doctor => doctor.Id);

            // ============================
            // Property Configurations
            // ============================

            builder.Property(doctor => doctor.ConsultationFee)
                   .HasPrecision(10, 2);


            // ============================
            // Relationships
            // ============================

            // One Doctor
            //      │
            //      ▼
            // One ApplicationUser
            builder.HasOne(doctor => doctor.User)
                   .WithOne(user => user.Doctor)
                   .HasForeignKey<Doctor>(doctor => doctor.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Many Doctors
            //      │
            //      ▼
            // One Department
            builder.HasOne(doctor => doctor.Department)
                   .WithMany(department => department.Doctors)
                   .HasForeignKey(doctor => doctor.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One Doctor
            //      │
            //      ▼
            // Many Appointments
            builder.HasMany(doctor => doctor.Appointments)
                   .WithOne(appointment => appointment.Doctor)
                   .HasForeignKey(appointment => appointment.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One Doctor
            //      │
            //      ▼
            // Many Working Hours
            builder.HasMany(doctor => doctor.WorkingHours)
                   .WithOne(workingHour => workingHour.Doctor)
                   .HasForeignKey(workingHour => workingHour.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            // One Doctor
            //      │
            //      ▼
            // Many Leave Records
            builder.HasMany(doctor => doctor.Leaves)
                   .WithOne(leave => leave.Doctor)
                   .HasForeignKey(leave => leave.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ============================
            // Indexes
            // ============================

            // Every doctor's license number must be unique.
            builder.HasIndex(doctor => doctor.LicenseNumber)
                   .IsUnique();

            // Improves searching doctors by specialization.
            builder.HasIndex(doctor => doctor.Specialization);

            // Improves filtering doctors by department.
            builder.HasIndex(doctor => doctor.DepartmentId);
        }
    }
}
