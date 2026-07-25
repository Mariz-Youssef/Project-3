namespace ClinicManagementSystem.backend.Features.Authentication.Models
{
    /// <summary>
    /// The subset of a validated Google ID token's claims needed for authentication.
    /// </summary>
    public class GoogleUserPayload
    {
        /// <summary>
        /// Google's stable, unique identifier for the user (the token's "sub" claim).
        /// </summary>
        public string Subject { get; init; } = string.Empty;

        /// <summary>
        /// The user's email address, as verified by Google.
        /// </summary>
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// The user's full display name.
        /// </summary>
        public string FullName { get; init; } = string.Empty;
    }
}
