namespace ClinicManagementSystem.backend.Features.Authentication.DTOs
{
    /// <summary>
    /// Response returned after a successful register, login, or token refresh.
    /// Contains user identity information plus the issued token pair.
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>Gets or sets the authenticated user's Id.</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the authenticated user's full name.</summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>Gets or sets the authenticated user's email.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Gets or sets the roles assigned to the user.</summary>
        public IList<string> Roles { get; set; } = new List<string>();

        /// <summary>Gets or sets the short-lived JWT access token.</summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>Gets or sets the UTC expiration of the access token.</summary>
        public DateTime AccessTokenExpiresAt { get; set; }

        /// <summary>Gets or sets the long-lived refresh token.</summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>Gets or sets the UTC expiration of the refresh token.</summary>
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
