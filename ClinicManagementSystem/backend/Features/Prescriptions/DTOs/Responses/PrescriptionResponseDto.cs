namespace ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Responses
{
    /// <summary>
    /// Represents prescription information returned
    /// to the client.
    /// </summary>
    public class PrescriptionResponseDto
    {
        public int Id { get; set; }

        public int MedicalRecordId { get; set; }

        public int AppointmentId { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public string MedicineName { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public string Frequency { get; set; } = string.Empty;

        public string Duration { get; set; } = string.Empty;

        public string? Instructions { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
