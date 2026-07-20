using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Authentication.Interfaces
{
    /// <summary>
    /// Generates access and refresh tokens for authenticated users.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a signed JWT access token containing the user's identity and role claims.
        /// </summary>
        /// <param name="user">The user to generate the token for.</param>
        /// <param name="roles">The roles to embed as claims.</param>
        /// <returns>The token string and its UTC expiration.</returns>
        (string Token, DateTime ExpiresAt) GenerateAccessToken(ApplicationUser user, IList<string> roles);

        /// <summary>
        /// Generates a new cryptographically random refresh token value.
        /// </summary>
        string GenerateRefreshTokenValue();
    }
}
