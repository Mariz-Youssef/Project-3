using ClinicManagementSystem.backend.Enums;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.DTOs;

/// <summary>
/// Represents a medical record payload returned to clients.
/// </summary>
public sealed class MedicalRecordResponse
{
    /// <summary>
    /// Gets or sets the medical record identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the appointment identifier.
    /// </summary>
    public int AppointmentId { get; set; }

    /// <summary>
    /// Gets or sets the doctor identifier.
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    /// Gets or sets the patient identifier.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Gets or sets the appointment date.
    /// </summary>
    public DateOnly AppointmentDate { get; set; }

    /// <summary>
    /// Gets or sets the appointment status.
    /// </summary>
    public AppointmentStatus AppointmentStatus { get; set; }

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

    /// <summary>
    /// Gets or sets last update timestamp.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
