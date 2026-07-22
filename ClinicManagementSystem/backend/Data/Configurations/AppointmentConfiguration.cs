using ClinicManagementSystem.backend.Data.Configurations.Base;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Data.Configurations
{
    /// <summary>
    /// Configures the Appointment entity.
    /// Defines database table mapping, relationships,
    /// indexes, enum conversions, and default values.
    /// </summary>
    public class AppointmentConfiguration : BaseEntityConfiguration<Appointment>
    {
        public override void Configure(EntityTypeBuilder<Appointment> builder)
        {
            base.Configure(builder);


            // ============================
            // Table Configuration
            // ============================

            builder.ToTable("Appointments");

            // ============================
            // Enum Conversions
            // ============================

            // Store AppointmentStatus as text instead of integer.
            builder.Property(appointment => appointment.Status)
                   .HasConversion<string>();

          

            // ============================
            // Relationships
            // ============================

            // One Doctor
            //      │
            //      ▼
            // Many Appointments
            builder.HasOne(appointment => appointment.Doctor)
                   .WithMany(doctor => doctor.Appointments)
                   .HasForeignKey(appointment => appointment.DoctorId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One Patient
            //      │
            //      ▼
            // Many Appointments
            builder.HasOne(appointment => appointment.Patient)
                   .WithMany(patient => patient.Appointments)
                   .HasForeignKey(appointment => appointment.PatientId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One Appointment
            //      │
            //      ▼
            // One Medical Record
            builder.HasOne(appointment => appointment.MedicalRecord)
                   .WithOne(medicalRecord => medicalRecord.Appointment)
                   .HasForeignKey<MedicalRecord>(medicalRecord => medicalRecord.AppointmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // Indexes
            // ============================

            // Improve searching appointments by doctor.
            builder.HasIndex(appointment => appointment.DoctorId);

            // Improve searching appointments by patient.
            builder.HasIndex(appointment => appointment.PatientId);

            // Improve filtering appointments by date.
            builder.HasIndex(appointment => appointment.AppointmentDate);

            // Improve filtering appointments by status.
            builder.HasIndex(appointment => appointment.Status);

            // Composite index used to efficiently query a doctor's schedule.
            builder.HasIndex(appointment => new
            {
                appointment.DoctorId,
                appointment.AppointmentDate,
                appointment.StartTime
            });
        }
    }
}
