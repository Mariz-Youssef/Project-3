using ClinicManagementSystem.backend.Features.Authentication.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Authentication.DTOs.Responses;

namespace ClinicManagementSystem.backend.Features.Authentication.Interfaces
{
    /// <summary>
    /// Handles authentication via Google Sign-In, including automatic
    /// account provisioning on first sign-in.
    /// </summary>
    public interface IGoogleAuthenticationService
    {
        /// <summary>
        /// Authenticates a user via Google Sign-In. Creates a new patient
        /// account automatically on first sign-in if no account exists.
        /// </summary>
        /// <param name="request">The Google ID token to validate.</param>
        /// <param name="ipAddress">The client IP address, recorded against the issued refresh token.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        /// <returns>The issued access/refresh token pair and user info.</returns>
        Task<AuthResponseDto> LoginAsync(
            GoogleLoginRequestDto request,
            string? ipAddress,
            CancellationToken cancellationToken = default);
    }
}
