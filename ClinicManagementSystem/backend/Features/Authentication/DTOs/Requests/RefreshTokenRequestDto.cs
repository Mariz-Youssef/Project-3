using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Authentication.DTOs.Requests
{
    /// <summary>
    /// Request payload for exchanging or revoking a refresh token.
    /// </summary>
    public class RefreshTokenRequestDto
    {
        /// <summary>Gets or sets the refresh token value previously issued to the client.</summary>
        [Required(ErrorMessage = "Refresh token is required.")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
