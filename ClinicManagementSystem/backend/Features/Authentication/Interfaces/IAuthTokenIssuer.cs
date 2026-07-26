using ClinicManagementSystem.backend.Features.Authentication.DTOs.Responses;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Authentication.Interfaces
{
    /// <summary>
    /// Issues access/refresh token pairs for an already-authenticated user,
    /// regardless of how that user was authenticated (password, Google, etc.).
    /// </summary>
    public interface IAuthTokenIssuer
    {
        /// <summary>
        /// Generates a new access/refresh token pair for the given user,
        /// persists the refresh token, and returns the full auth response.
        /// </summary>
        /// <param name="user">The authenticated user.</param>
        /// <param name="ipAddress">The client IP address, recorded against the issued refresh token.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        /// <returns>The issued access/refresh token pair and user info.</returns>
        Task<AuthResponseDto> IssueTokensAsync(
            ApplicationUser user,
            string? ipAddress,
            CancellationToken cancellationToken = default);
    }
}
