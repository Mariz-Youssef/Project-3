using ClinicManagementSystem.backend.Common;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents a doctor working in the clinic.
    /// Stores professional information only.
    /// Authentication information is stored in ApplicationUser.
    /// </summary>
    public class Doctor : BaseEntity
    {
        /// <summary>
        /// Gets or sets the related ApplicationUser Id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the department Id.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the doctor's specialization.
        /// </summary>
        [Required(ErrorMessage = "Specialization is required.")]
        [MaxLength(100, ErrorMessage = "Specialization cannot exceed 100 characters.")]
        public string Specialization { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the doctor's medical license number.
        /// </summary>
        [Required(ErrorMessage = "Medical license number is required.")]
        [MaxLength(50, ErrorMessage = "Medical license number cannot exceed 50 characters.")]
        public string LicenseNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the doctor's years of experience.
        /// </summary>
        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60.")]
        public int YearsOfExperience { get; set; }

        /// <summary>
        /// Gets or sets the consultation fee.
        /// </summary>
        [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100000.")]
        public decimal ConsultationFee { get; set; }

        // ============================
        // Navigation Properties
        // ============================

        /// <summary>
        /// Gets the user account associated with this doctor.
        /// One-to-One relationship.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Gets the department where the doctor works.
        /// Many doctors belong to one department.
        /// </summary>
        public Department Department { get; set; } = null!;

        /// <summary>
        /// Gets all appointments assigned to this doctor.
        /// One doctor can have many appointments.
        /// </summary>
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        /// <summary>
        /// Gets the doctor's weekly working schedule.
        /// </summary>
        public ICollection<DoctorWorkingHour> WorkingHours { get; set; } = new List<DoctorWorkingHour>();

        /// <summary>
        /// Gets the doctor's leave records.
        /// </summary>
        public ICollection<DoctorLeave> Leaves { get; set; } = new List<DoctorLeave>();
    }
}
