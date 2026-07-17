using ClinicManagementSystem.backend.Common;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents a medical department in the clinic.
    /// Each department can contain multiple doctors.
    /// Examples include Cardiology, Pediatrics, Neurology, and Dentistry.
    /// </summary>
    public class Department:BaseEntity
    {
        /// <summary>
        /// Gets or sets the department name.
        /// </summary>
        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief description of the department.
        /// </summary>
        [MaxLength(500, ErrorMessage = "Department description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        // ============================
        // Navigation Properties
        // ============================

        /// <summary>
        /// Gets or sets the doctors working in this department.
        /// Represents a one-to-many relationship between Department and Doctor.
        /// One Department can have multiple Doctors, but each Doctor belongs to one Department.
        /// </summary>

        // Initializing the collection to avoid null reference exceptions when adding doctors.
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    }
}
