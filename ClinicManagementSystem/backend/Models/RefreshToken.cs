using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents a refresh token issued to a user, allowing the client
    /// to obtain a new access token without re-entering credentials.
    /// Supports rotation and revocation for security.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>Gets or sets the primary key.</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the random, unguessable token value.</summary>
        [Required]
        [MaxLength(200)]
        public string Token { get; set; } = string.Empty;

        /// <summary>Gets or sets the Id of the owning ApplicationUser.</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the UTC creation timestamp.</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Gets or sets the UTC expiration timestamp.</summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>Gets or sets the IP address that created the token.</summary>
        [MaxLength(45)]
        public string? CreatedByIp { get; set; }

        /// <summary>Gets or sets the UTC timestamp when the token was revoked, if any.</summary>
        public DateTime? RevokedAt { get; set; }

        /// <summary>Gets or sets the IP address that revoked the token, if any.</summary>
        [MaxLength(45)]
        public string? RevokedByIp { get; set; }

        /// <summary>Gets or sets the token that replaced this one after rotation, if any.</summary>
        [MaxLength(200)]
        public string? ReplacedByToken { get; set; }

        /// <summary>Gets a value indicating whether the token has expired.</summary>
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        /// <summary>Gets a value indicating whether the token is still usable (not revoked, not expired).</summary>
        [NotMapped]
        public bool IsActive => RevokedAt is null && !IsExpired;

        /// <summary>Gets or sets the owning user.</summary>
        public ApplicationUser User { get; set; } = null!;
    }
}
