using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Appointments.DTOs.Requests
{
    /// <summary>
    /// Represents the request to book a new appointment.
    /// </summary>
    public class CreateAppointmentRequestDto
    {
        /// <summary>
        /// Doctor identifier.
        /// </summary>
        [Required(ErrorMessage = "Doctor Id is Required")]
        public int DoctorId { get; set; }

        /// <summary>
        /// Appointment date.
        /// </summary>
        [Required(ErrorMessage = "Appointment Date is Required")]
        public DateOnly AppointmentDate { get; set; }

        /// <summary>
        /// Appointment start time.
        /// </summary>
        [Required(ErrorMessage = "StartTime of Appointment is Required")]
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// Appointment end time.
        /// </summary>
        [Required(ErrorMessage = "EndTime of Appointment is Required")]
        public TimeOnly EndTime { get; set; }

        /// <summary>
        /// Reason for the appointment.
        /// </summary>
        [Required(ErrorMessage ="Reasons for attending this appointment is Required")]
        [MaxLength(500,ErrorMessage ="Reasons maximum length should not exceeds 500 character ")]
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Additional notes.
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
