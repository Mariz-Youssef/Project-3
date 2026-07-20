using ClinicManagementSystem.backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents a patient registered in the clinic.
    /// Stores medical and personal information.
    /// Authentication information is stored in ApplicationUser.
    /// </summary>
    public class Patient:BaseEntity
    {
        /// <summary>
        /// Gets or sets the related ApplicationUser Id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the patient's date of birth.
        /// </summary>
        public DateOnly DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the patient's gender.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the patient's blood group.
        /// </summary>
        public BloodGroup BloodGroup { get; set; }

        /// <summary>
        /// Gets or sets the patient's home address.
        /// </summary>
        [Required(ErrorMessage = "Home address is required.")]
        [MaxLength(250, ErrorMessage = "Home address cannot exceed 250 characters.")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the patient's allergies.
        /// Leave empty if there are no known allergies.
        /// </summary>
        [MaxLength(500, ErrorMessage = "Allergies cannot exceed 500 characters.")]
        public string? Allergies { get; set; }

        /// <summary>
        /// Gets or sets the emergency contact person's name.
        /// </summary>
        [Required(ErrorMessage = "Emergency contact name is required.")]
        [MaxLength(100, ErrorMessage = "Emergency contact name cannot exceed 100 characters.")]
        public string EmergencyContactName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the emergency contact phone number.
        /// </summary>
        [Required(ErrorMessage = "Emergency contact phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        // ============================
        // Navigation Properties
        // ============================

        /// <summary>
        /// Gets the user account associated with this patient.
        /// Represents a one-to-one relationship.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// Gets all appointments booked by this patient.
        /// Represents a one-to-many relationship.
        /// one patient can have many appointments.
        /// </summary>
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
