namespace ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Responses
{
    /// <summary>
    /// Represents a medical record returned to the client.
    /// </summary>
    public class MedicalRecordResponseDto
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
        /// Gets or sets the doctor's full name.
        /// </summary>
        public string DoctorName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the patient's full name.
        /// </summary>
        public string PatientName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the appointment date.
        /// </summary>
        public DateOnly AppointmentDate { get; set; }

        /// <summary>
        /// Gets or sets the doctor's diagnosis.
        /// </summary>
        public string Diagnosis { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the doctor's notes.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the recommended follow-up date.
        /// </summary>
        public DateOnly? FollowUpDate { get; set; }

        /// <summary>
        /// Gets or sets the record creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
