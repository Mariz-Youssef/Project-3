using ClinicManagementSystem.backend.Common.Constants;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    /// <summary>
    /// Extension methods for registering role-based authorization policies.
    /// </summary>
    public static class AuthorizationServiceExtensions
    {
        /// <summary>
        /// Registers authorization policies matching the system's four roles
        /// (Admin, Doctor, Receptionist, Patient) plus common combinations.
        /// </summary>
        /// <param name="services">The service collection to add to.</param>
        /// <returns>The same service collection, for chaining.</returns>
        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(RoleNames.Admin));
                options.AddPolicy("DoctorOnly", policy => policy.RequireRole(RoleNames.Doctor));
                options.AddPolicy("PatientOnly", policy => policy.RequireRole(RoleNames.Patient));
                options.AddPolicy("AdminOrDoctor", policy =>
                    policy.RequireRole(RoleNames.Admin, RoleNames.Doctor));

                options.AddPolicy("AdminOrDoctorOrPatient",
                    policy => policy.RequireRole(RoleNames.Admin, RoleNames.Doctor, RoleNames.Patient));

            });

            return services;
        }
    }
}

