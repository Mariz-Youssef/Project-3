using ClinicManagementSystem.backend.Common;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents a doctor's leave or vacation period.
    /// Doctors cannot receive appointments during this period.
    /// </summary>
    public class DoctorLeave: BaseEntity
    {
        /// <summary>
        /// Gets or sets the doctor identifier.
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Gets or sets the leave start date.
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Gets or sets the leave end date.
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Gets or sets the reason for the leave.
        /// </summary>
        [MaxLength(300, ErrorMessage = "Reason cannot exceed 300 characters.")]
        public string? Reason { get; set; }

        // ============================
        // Navigation Properties
        // ============================

        /// <summary>
        /// Gets the doctor associated with this leave.
        /// Represents a many-to-one relationship.
        /// </summary>
        public Doctor Doctor { get; set; } = null!;
    }
}
