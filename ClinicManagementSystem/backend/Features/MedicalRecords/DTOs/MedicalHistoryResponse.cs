namespace ClinicManagementSystem.backend.Features.MedicalRecords.DTOs;

/// <summary>
/// Represents a patient medical history entry.
/// </summary>
public sealed class MedicalHistoryResponse
{
    /// <summary>
    /// Gets or sets the medical record identifier.
    /// </summary>
    public int MedicalRecordId { get; set; }

    /// <summary>
    /// Gets or sets the appointment identifier.
    /// </summary>
    public int AppointmentId { get; set; }

    /// <summary>
    /// Gets or sets the doctor identifier.
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    /// Gets or sets the doctor full name.
    /// </summary>
    public string DoctorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the appointment date.
    /// </summary>
    public DateOnly AppointmentDate { get; set; }

    /// <summary>
    /// Gets or sets diagnosis details.
    /// </summary>
    public string Diagnosis { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the follow-up date.
    /// </summary>
    public DateOnly? FollowUpDate { get; set; }

    /// <summary>
    /// Gets or sets creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
