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
        /// Registers a new user account and returns a token pair for immediate login.
        /// </summary>
        /// <param name="request">The registration details.</param>
        /// <param name="ipAddress">The client IP address, recorded against the issued refresh token.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
       
        Task<AuthServiceResult<RegisterResponseDto>> RegisterAsync(
    RegisterRequestDto request,
    CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates credentials and returns a token pair on success.
        /// </summary>
        /// <param name="request">The login credentials.</param>
        /// <param name="ipAddress">The client IP address, recorded against the issued refresh token.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task<AuthServiceResult<AuthResponseDto>> LoginAsync(
            LoginRequestDto request,
            string? ipAddress,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates a refresh token, rotates it (revokes the old, issues a new one),
        /// and returns a fresh access/refresh token pair.
        /// </summary>
        /// <param name="refreshToken">The refresh token presented by the client.</param>
        /// <param name="ipAddress">The client IP address, recorded against the rotation.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task<AuthServiceResult<AuthResponseDto>> RefreshTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes a refresh token so it can no longer be used.
        /// </summary>
        /// <param name="refreshToken">The refresh token to revoke.</param>
        /// <param name="ipAddress">The client IP address that requested the revocation.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task<AuthServiceResult<bool>> RevokeTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Lightweight result wrapper used within the Auth service layer,
    /// decoupled from the HTTP response shape used by controllers.
    /// </summary>
    /// <typeparam name="T">The type of the result payload.</typeparam>
    public class AuthServiceResult<T>
    {
        /// <summary>Gets a value indicating whether the operation succeeded.</summary>
        public bool Success { get; init; }

        /// <summary>Gets a human-readable message describing the result.</summary>
        public string Message { get; init; } = string.Empty;

        /// <summary>Gets the result payload, if any.</summary>
        public T? Data { get; init; }

        /// <summary>Gets a list of error details, if any.</summary>
        public IList<string>? Errors { get; init; }

        /// <summary>Creates a successful result.</summary>
        public static AuthServiceResult<T> Ok(T data, string message = "Success") =>
            new() { Success = true, Message = message, Data = data };

        /// <summary>Creates a failed result.</summary>
        public static AuthServiceResult<T> Fail(string message, IList<string>? errors = null) =>
            new() { Success = false, Message = message, Errors = errors };
    }
}
