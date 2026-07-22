using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.Authentication.DTOs;

namespace ClinicManagementSystem.backend.Features.Authentication.Interfaces
{
    /// <summary>
    /// Handles user registration, login, and the refresh-token lifecycle
    /// (issuance, rotation, and revocation).
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new patient login identity (Users table + Patient role only).
        /// The patient must complete their profile separately after logging in.
        /// </summary>
        /// <param name="request">The registration details.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task<ApiResponse<RegisterResponseDto>> RegisterPatientAsync(
            RegisterRequestDto request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new doctor login identity (Users table + Doctor role only).
        /// Restricted to Admin callers. The Doctor profile (specialization, license,
        /// department, etc.) must be completed separately via the Doctors feature.
        /// </summary>
        /// <param name="request">The doctor account details.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task<ApiResponse<DoctorAccountResponseDto>> CreateDoctorAccountAsync(
            CreateDoctorAccountRequestDto request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates credentials and returns a token pair on success.
        /// </summary>
        /// <param name="request">The login credentials.</param>
        /// <param name="ipAddress">The client IP address, recorded against the issued refresh token.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task<ApiResponse<AuthResponseDto>> LoginAsync(
            LoginRequestDto request,
            string? ipAddress,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates a refresh token, rotates it, and returns a fresh token pair.
        /// </summary>
        /// <param name="refreshToken">The refresh token presented by the client.</param>
        /// <param name="ipAddress">The client IP address, recorded against the rotation.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes a refresh token so it can no longer be used.
        /// </summary>
        /// <param name="refreshToken">The refresh token to revoke.</param>
        /// <param name="ipAddress">The client IP address that requested the revocation.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task<ApiResponse<bool>> RevokeTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default);
    }
}
