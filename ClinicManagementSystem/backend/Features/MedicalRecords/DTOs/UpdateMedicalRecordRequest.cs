using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.DTOs;

/// <summary>
/// Represents the payload required to update a medical record.
/// </summary>
public sealed class UpdateMedicalRecordRequest
{
    /// <summary>
    /// Gets or sets the diagnosis entered by the doctor.
    /// </summary>
    [Required(ErrorMessage = "Diagnosis is required.")]
    [MaxLength(1000, ErrorMessage = "Diagnosis cannot exceed 1000 characters.")]
    public string Diagnosis { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [MaxLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters.")]
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the optional follow-up date.
    /// </summary>
    public DateOnly? FollowUpDate { get; set; }
}
