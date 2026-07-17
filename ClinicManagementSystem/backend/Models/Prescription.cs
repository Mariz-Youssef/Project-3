using ClinicManagementSystem.backend.Common;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents a single prescribed medication
    /// associated with a medical record.
    /// </summary>
    public class Prescription: BaseEntity
    {
        /// <summary>
        /// Gets or sets the related medical record Id.
        /// </summary>
        public int MedicalRecordId { get; set; }

        /// <summary>
        /// Gets or sets the medicine name.
        /// </summary>
        [Required(ErrorMessage = "Medicine name is required.")]
        [MaxLength(200, ErrorMessage = "Medicine name cannot exceed 200 characters.")]
        public string MedicineName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the medicine dosage.
        /// Example: 500 mg
        /// </summary>
        [Required(ErrorMessage = "Dosage is required.")]
        [MaxLength(100, ErrorMessage = "Dosage cannot exceed 100 characters.")]
        public string Dosage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets how often the medicine should be taken.
        /// Example: Twice Daily.
        /// </summary>
        [Required(ErrorMessage = "Frequency is required.")]
        [MaxLength(100, ErrorMessage = "Frequency cannot exceed 100 characters.")]
        public string Frequency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the treatment duration.
        /// Example: 7 Days.
        /// </summary>
        [Required(ErrorMessage = "Duration is required.")]
        [MaxLength(100, ErrorMessage = "Duration cannot exceed 100 characters.")]
        public string Duration { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets additional instructions.
        /// </summary>
        [MaxLength(500, ErrorMessage = "Instructions cannot exceed 500 characters.")]
        public string? Instructions { get; set; }

        // ============================
        // Navigation Properties
        // ============================

        /// <summary>
        /// Gets the medical record associated with this prescription.
        /// Represents a many-to-one relationship.
        /// </summary>
        public MedicalRecord MedicalRecord { get; set; } = null!;
    }
}
