using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Patients.DTOs;

public sealed class CreatePatientDto
{
    [PastDate]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string Gender { get; set; } = string.Empty;

    [MaxLength(3)]
    public string BloodGroup { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    public string? Allergies { get; set; }

    public string? MedicalNotes { get; set; }

    [MaxLength(100)]
    public string EmergencyContactName { get; set; } = string.Empty;

    [RegularExpression(@"^\+?[1-9]\d{1,14}$")]
    public string EmergencyContactPhone { get; set; } = string.Empty;
}