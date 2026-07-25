using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Requests
{
    /// <summary>
    /// Represents the request used to create a medical record
    /// for a completed appointment.
    /// </summary>
    public class CreateMedicalRecordRequestDto
    {
        /// <summary>
        /// Gets or sets the completed appointment identifier.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Appointment ID must be greater than zero.")]
        public int AppointmentId { get; set; }

        /// <summary>
        /// Gets or sets the doctor's diagnosis.
        /// </summary>
        [Required(ErrorMessage = "Diagnosis is required.")]
        [MaxLength(2000, ErrorMessage = "Diagnosis cannot exceed 2000 characters.")]
        public string Diagnosis { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional doctor's notes.
        /// </summary>
        [MaxLength(4000, ErrorMessage = "Notes cannot exceed 4000 characters.")]
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the recommended follow-up date.
        /// </summary>
        public DateOnly? FollowUpDate { get; set; }
    }
}
