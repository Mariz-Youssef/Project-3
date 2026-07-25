using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Authentication.DTOs.Requests
{
    /// <summary>
    /// Request payload for changing the authenticated user's password.
    /// </summary>
    public class ChangePasswordRequestDto
    {
        /// <summary>
        /// The user's current password, required to verify identity before changing it.
        /// </summary>
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// The new password to set.
        /// </summary>
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Confirmation of the new password. Must match <see cref="NewPassword"/>.
        /// </summary>
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
