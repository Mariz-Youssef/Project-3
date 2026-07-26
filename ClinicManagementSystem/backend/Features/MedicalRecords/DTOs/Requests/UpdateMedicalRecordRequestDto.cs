using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Requests
{
    /// <summary>
    /// Represents the request used to update an existing medical record.
    /// </summary>
    public class UpdateMedicalRecordRequestDto
    {
        /// <summary>
        /// Gets or sets the updated diagnosis.
        /// </summary>
        [Required(ErrorMessage = "Diagnosis is required.")]
        [MaxLength(2000, ErrorMessage = "Diagnosis cannot exceed 2000 characters.")]
        public string Diagnosis { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated doctor's notes.
        /// </summary>
        [MaxLength(4000, ErrorMessage = "Notes cannot exceed 4000 characters.")]
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the updated follow-up date.
        /// </summary>
        public DateOnly? FollowUpDate { get; set; }
    }
}
