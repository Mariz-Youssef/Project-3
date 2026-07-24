using ClinicManagementSystem.backend.Enums;

namespace ClinicManagementSystem.backend.Features.Appointments.DTOs.Responses
{
    public class AppointmentDetailsResponseDto
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public DateOnly AppointmentDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public AppointmentStatus Status { get; set; }

        public string Reason { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
