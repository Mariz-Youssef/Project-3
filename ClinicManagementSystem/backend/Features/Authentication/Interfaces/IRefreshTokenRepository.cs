using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Authentication.Interfaces
{
    /// <summary>
    /// Provides data access for <see cref="RefreshToken"/> entities.
    /// </summary>
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Retrieves a refresh token by its value, including the owning user.
        /// </summary>
        /// <param name="token">The token value to look up.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to add.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

        /// <summary>
        /// Persists any pending changes to the data store.
        /// </summary>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
