using ClinicManagementSystem.backend.Features.Authentication.Models;

namespace ClinicManagementSystem.backend.Features.Authentication.Interfaces
{
    /// <summary>
    /// Validates Google Sign-In ID tokens and extracts the signed-in user's payload.
    /// </summary>
    public interface IGoogleTokenValidator
    {
        /// <summary>
        /// Validates the given Google ID token's signature, expiry, and audience.
        /// </summary>
        /// <param name="idToken">The ID token issued by Google.</param>
        /// <returns>The validated Google payload containing the user's email, name, and subject ID.</returns>
        /// <exception cref="Common.Exceptions.CustomExceptions.UnauthorizedException">
        /// Thrown if the token is invalid, expired, or has the wrong audience.
        /// </exception>
        Task<GoogleUserPayload> ValidateAsync(string idToken);
    }
}
