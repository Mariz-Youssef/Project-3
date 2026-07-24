using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using System.Security.Claims;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    /// <summary>
    /// Extension methods for extracting typed values from <see cref="ClaimsPrincipal"/>.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Retrieves the authenticated user's ID from the <see cref="ClaimTypes.NameIdentifier"/> claim.
        /// </summary>
        /// <param name="user">The claims principal, typically <c>HttpContext.User</c>.</param>
        /// <returns>The authenticated user's ID.</returns>
        /// <exception cref="UnauthorizedException">Thrown if the claim is missing or not a valid integer.</exception>
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(idClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid user token.");
            }

            return userId;
        }
    }
}
