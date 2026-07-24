using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Appointments.DTOs.Requests
{
    public class UpdateAppointmentRequestDto
    {
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
        [Required(ErrorMessage = "Reasons for attending this appointment is Required")]
        [MaxLength(500, ErrorMessage = "Reasons maximum length should not exceeds 500 character ")]
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Additional notes.
        /// </summary>
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
