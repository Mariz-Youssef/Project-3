using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using ClinicManagementSystem.backend.Features.Authentication.Repository;
using ClinicManagementSystem.backend.Features.Authentication.Services;

namespace ClinicManagementSystem.backend.Features.Authentication.Extensions
{
    /// <summary>
    /// Registers service-layer dependencies for the Authentication feature.
    /// </summary>
    public static class AuthServiceExtensions
    {
        /// <summary>
        /// Registers <see cref="IAuthService"/> and <see cref="ITokenService"/>
        /// for the Authentication feature.
        /// </summary>
        public static IServiceCollection AddAuthFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
