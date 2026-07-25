using ClinicManagementSystem.backend.Common.Settings;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    /// <summary>
    /// Extension methods for registering Google Sign-In configuration.
    /// </summary>
    public static class GoogleAuthenticationServiceExtensions
    {
        /// <summary>
        /// Binds <see cref="GoogleSettings"/> from configuration.
        /// </summary>
        /// <param name="services">The service collection to add to.</param>
        /// <param name="configuration">The application configuration.</param>
        /// <returns>The same service collection, for chaining.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the "GoogleSettings" configuration section is missing.
        /// </exception>
        public static IServiceCollection AddGoogleAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<GoogleSettings>(configuration.GetSection(GoogleSettings.SectionName));

            _ = configuration.GetSection(GoogleSettings.SectionName).Get<GoogleSettings>()
                ?? throw new InvalidOperationException("GoogleSettings section is missing from configuration.");

            return services;
        }
    }
}
