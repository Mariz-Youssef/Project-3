using System.Security.Claims;

namespace ClinicManagementSystem.backend.Common.Services.Interfaces
{
    /// <summary>
    /// Provides information about the currently authenticated user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the authenticated user's Id.
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// Gets the authenticated user's username.
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Gets the authenticated user's email.
        /// </summary>
        string? Email { get; }

        /// <summary>
        /// Indicates whether the current request is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets all roles assigned to the authenticated user.
        /// </summary>
        IReadOnlyList<string> Roles { get; }

        /// <summary>
        /// Determines whether the current user belongs to the specified role.
        /// </summary>
        bool IsInRole(string role);

        /// <summary>
        /// Returns the current ClaimsPrincipal.
        /// </summary>
        ClaimsPrincipal User { get; }
    }
}
