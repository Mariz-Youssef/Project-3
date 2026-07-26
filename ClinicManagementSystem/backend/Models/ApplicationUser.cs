using Microsoft.AspNetCore.Identity;
using System.Numerics;

namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents the authenticated user in the Clinic Management System.
    /// This entity extends ASP.NET Core Identity to support authentication,
    /// authorization, auditing, and soft delete.
    /// 
    /// A user can have one of the following roles:
    /// - Admin
    /// - Receptionist
    /// - Doctor
    /// - Patient
    /// 
    /// Additional business information for doctors and patients
    /// is stored in the Doctor and Patient entities.
    /// </summary>
    public class ApplicationUser: IdentityUser<int>
    {
        /// <summary>
        /// Gets or sets the user's full name.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the user has been soft deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user account was soft deleted.
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the user account was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // ============================
        // Navigation Properties
        // ============================

        /// <summary>
        /// Gets or sets the doctor profile associated with this user.
        /// This property is only populated when the user has the Doctor role.
        /// </summary>
        public Doctor? Doctor { get; set; }

        /// <summary>
        /// Gets or sets the patient profile associated with this user.
        /// This property is only populated when the user has the Patient role.
        /// </summary>
        public Patient? Patient { get; set; }
        /// <summary>
        /// Gets or sets the refresh tokens issued to this user.
        /// </summary>
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
