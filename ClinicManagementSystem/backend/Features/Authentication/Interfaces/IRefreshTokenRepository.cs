using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Authentication.Interfaces
{
    /// <summary>
    /// Provides data access for <see cref="RefreshToken"/> entities.
    /// </summary>
    public interface IRefreshTokenRepository
    {
        /// <param name="token">The token value to look up.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        /// <returns>The matching <see cref="RefreshToken"/>, or <c>null</c> if not found.</returns>
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to add.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        /// <summary>
        /// Revokes every descendant token in the rotation chain starting from
        /// (but not including) the given token, used when reuse of a revoked
        /// token indicates possible theft.
        /// </summary>
        /// <param name="token">The token whose descendants should be revoked.</param>
        /// <param name="ipAddress">The IP address that triggered the revocation.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task RevokeDescendantsAsync(RefreshToken token, string? ipAddress, CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes all active refresh tokens belonging to the given user.
        /// Used after a password change to invalidate existing sessions.
        /// </summary>
        /// <param name="userId">The ID of the user whose tokens should be revoked.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task RevokeAllForUserAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Persists any pending changes to the data store.
        /// </summary>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
