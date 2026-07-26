using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Authentication.DTOs.Requests
{
    /// <summary>
    /// Request payload for authenticating an existing user.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>Gets or sets the user's email address.</summary>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>Gets or sets the account password.</summary>
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}
