using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents the medical record created after a completed appointment.
    /// It contains the doctor's diagnosis and treatment notes after examining the patient.
    /// </summary>
    public class MedicalRecord:BaseEntity
    {
        /// <summary>
        /// Gets or sets the related appointment Id.
        /// </summary>
        public int AppointmentId { get; set; }

        /// <summary>
        /// Gets or sets the diagnosis determined by the doctor.
        /// </summary>
        [Required(ErrorMessage = "Diagnosis is required.")]
        [MaxLength(1000, ErrorMessage = "Diagnosis cannot exceed 1000 characters.")]
        public string Diagnosis { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional medical notes.
        /// </summary>
        [MaxLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters.")]
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the recommended follow-up date.
        /// </summary>
        public DateOnly? FollowUpDate { get; set; }

        // ============================
        // Navigation Properties
        // ============================

        /// <summary>
        /// Gets the appointment associated with this medical record.
        /// Represents a one-to-one relationship.
        /// </summary>
        public Appointment Appointment { get; set; } = null!;

        /// <summary>
        /// Gets all prescriptions issued during this visit.
        /// Represents a one-to-many relationship.
        /// </summary>
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    }
}
