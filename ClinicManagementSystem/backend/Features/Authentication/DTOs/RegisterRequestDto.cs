using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Authentication.DTOs
{
    /// <summary>
    /// Request payload for registering a new user account.
    /// </summary>
    public class RegisterRequestDto
    {
        /// <summary>Gets or sets the user's full name.</summary>
        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>Gets or sets the user's email address. Used as the username.</summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Gets or sets the account password.</summary>
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;

        /// <summary>Gets or sets the password confirmation. Must match Password.</summary>
        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
