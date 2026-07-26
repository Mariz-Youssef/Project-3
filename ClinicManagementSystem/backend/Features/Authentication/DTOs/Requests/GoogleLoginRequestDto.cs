using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Authentication.DTOs.Requests
{
    /// <summary>
    /// Request payload for authenticating via Google Sign-In.
    /// </summary>
    public class GoogleLoginRequestDto
    {
        /// <summary>
        /// The ID token issued by Google after the client completes Google Sign-In.
        /// </summary>
        [Required]
        public string IdToken { get; set; } = string.Empty;
    }
}
