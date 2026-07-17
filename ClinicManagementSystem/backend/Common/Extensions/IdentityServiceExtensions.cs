using ClinicManagementSystem.backend.Common.Data;
using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    /// <summary>
    /// Extension methods for registering ASP.NET Core Identity.
    /// </summary>
    public static class IdentityServiceExtensions
    {
        /// <summary>
        /// Registers Identity services.
        /// </summary>
        public static IServiceCollection AddIdentityServices(
            this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
                {
                    // Password Settings
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;

                    // User Settings
                    options.User.RequireUniqueEmail = true;

                    // Lockout Settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
