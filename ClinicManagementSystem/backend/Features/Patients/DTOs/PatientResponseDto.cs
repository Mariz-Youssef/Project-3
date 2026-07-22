namespace ClinicManagementSystem.backend.Features.Patients.DTOs;

public sealed class PatientResponseDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string BloodGroup { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Allergies { get; set; } = string.Empty;

    public string? MedicalNotes { get; set; }

    public string EmergencyContactName { get; set; } = string.Empty;

    public string EmergencyContactPhone { get; set; } = string.Empty;
}