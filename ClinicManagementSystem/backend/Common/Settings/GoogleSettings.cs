namespace ClinicManagementSystem.backend.Common.Settings
{
    /// <summary>
    /// Configuration required to validate Google Sign-In ID tokens.
    /// </summary>
    public sealed class GoogleSettings
    {
        /// <summary>
        /// The configuration section name in appsettings.json.
        /// </summary>
        public const string SectionName = "GoogleSettings";
        /// <summary>
        /// The OAuth 2.0 client ID registered in Google Cloud Console.
        /// Used to verify the token's audience claim.
        /// </summary>
        public string ClientId { get; set; } = string.Empty;
    }
}
