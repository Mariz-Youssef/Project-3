using ClinicManagementSystem.backend.Common;
using ClinicManagementSystem.backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents an appointment booked by a patient with a doctor.
    /// This entity is the core of the clinic management system.
    /// </summary>
    public class Appointment : BaseEntity
    {
        /// <summary>
        /// Gets or sets the doctor's Id.
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Gets or sets the patient's Id.
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the appointment date.
        /// </summary>
        public DateOnly AppointmentDate { get; set; }

        /// <summary>
        /// Gets or sets the appointment start time.
        /// </summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// Gets or sets the appointment end time.
        /// </summary>
        public TimeOnly EndTime { get; set; }

        /// <summary>
        /// Gets or sets the current appointment status.
        /// </summary>
        public AppointmentStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the reason for the appointment.
        /// </summary>
        [Required(ErrorMessage = "Reason is required.")]
        [MaxLength(500, ErrorMessage = "Reason cannot exceed 500 characters.")]
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Additional notes entered by the receptionist or doctor.
        /// </summary>
        [MaxLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        public string? Notes { get; set; }

        // ============================
        // Navigation Properties
        // ============================

        /// <summary>
        /// Gets the doctor assigned to this appointment.
        /// </summary>
        public Doctor Doctor { get; set; } = null!;

        /// <summary>
        /// Gets the patient who booked this appointment.
        /// </summary>
        public Patient Patient { get; set; } = null!;

        /// <summary>
        /// Gets the medical record generated after completing the appointment.
        /// </summary>
        public MedicalRecord? MedicalRecord { get; set; }
    }
}
