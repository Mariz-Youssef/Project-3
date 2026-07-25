using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using ClinicManagementSystem.backend.Features.Authentication.Repository;
using ClinicManagementSystem.backend.Features.Authentication.Services;
using ClinicManagementSystem.backend.Features.Authentication.Validators;

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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();
            services.AddScoped<IAuthTokenIssuer, AuthTokenIssuer>();
            services.AddScoped<IGoogleAuthenticationService, GoogleAuthenticationService>();
           

            return services;
        }
    }
}
