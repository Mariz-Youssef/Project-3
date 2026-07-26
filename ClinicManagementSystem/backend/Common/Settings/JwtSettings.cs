namespace ClinicManagementSystem.backend.Common.Settings
{
    /// <summary>
    /// Strongly typed representation of the "JwtSettings" section
    /// in appsettings.json, used to configure JWT generation and validation.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
      /// The configuration section name this class binds to.
      /// </summary>
        public const string SectionName = "JwtSettings";

        /// <summary>
        /// Gets or sets the symmetric secret key used to sign access tokens.
        /// Must be at least 32 characters long for HMAC-SHA256.
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token issuer (the API that issues the token).
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the intended audience of the token.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of minutes an access token remains valid.
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; }

        /// <summary>
        /// Gets or sets the number of days a refresh token remains valid.
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; }
    }
}
