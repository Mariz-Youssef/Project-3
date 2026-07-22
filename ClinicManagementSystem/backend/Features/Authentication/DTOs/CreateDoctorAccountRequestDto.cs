using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Authentication.DTOs
{
    /// <summary>
    /// Request payload used by an Admin to create a Doctor's login identity.
    /// This creates only the Users table row and assigns the Doctor role;
    /// it does not create the Doctor profile (specialization, license, etc.).
    /// The Doctor profile must be completed separately via the Doctors feature.
    /// </summary>
    public class CreateDoctorAccountRequestDto
    {
        /// <summary>Gets or sets the doctor's full name.</summary>
        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>Gets or sets the doctor's email address. Used as the username.</summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the initial password for the account.
        /// The doctor should be advised to change this on first login.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
