using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Requests
{
    /// <summary>
    /// Represents the information required to create
    /// a new prescription.
    /// </summary>
    public class CreatePrescriptionRequestDto
    {
        /// <summary>
        /// Gets or sets the medical record identifier.
        /// </summary>
        [Required(ErrorMessage ="Medical Record Id field is required")]
        public int MedicalRecordId { get; set; }

        /// <summary>
        /// Gets or sets the medicine name.
        /// </summary>
        [Required(ErrorMessage = "Medicine Name field is required")]
        [MaxLength(200,ErrorMessage = "Medicine Name maximum length can not exceeds 200 Character")]
        public string MedicineName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the dosage.
        /// </summary>
        [Required(ErrorMessage ="Dosage field is required")]
        [MaxLength(100, ErrorMessage = "Dosage maximum length can not exceeds 100 Character")]
        public string Dosage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        [Required(ErrorMessage = "Frequency field is required ")]
        [MaxLength(100,ErrorMessage = "Frequency maximum length can not exceeds 100 Character")]
        public string Frequency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the treatment duration.
        /// </summary>
        [Required(ErrorMessage = "Duration field is required ")]
        [MaxLength(100,ErrorMessage = "Duration maximum length can not exceeds 100 Character")]
        public string Duration { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional instructions.
        /// </summary>
        [MaxLength(500,ErrorMessage = "Instructions maximum length can not exceeds 500 Character")]
        public string? Instructions { get; set; }
    }
}
