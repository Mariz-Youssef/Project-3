using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Authentication.Repository
{
    /// <inheritdoc cref="IRefreshTokenRepository"/>

    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
        }

        /// <inheritdoc />
        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }

        /// <inheritdoc />
        public async Task RevokeDescendantsAsync(
            RefreshToken token,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(token.ReplacedByToken))
            {
                return;
            }

            var child = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token.ReplacedByToken, cancellationToken);

            if (child is null)
            {
                return;
            }

            if (child.IsActive)
            {
                child.RevokedAt = DateTime.UtcNow;
                child.RevokedByIp = ipAddress;
            }

            await RevokeDescendantsAsync(child, ipAddress, cancellationToken);
        }

        /// <inheritdoc />
        public async Task RevokeAllForUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
                .ToListAsync(cancellationToken);

            foreach (var token in activeTokens)
            {
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
