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
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("DoctorOnly", policy => policy.RequireRole("Doctor"));
                options.AddPolicy("ReceptionistOnly", policy => policy.RequireRole("Receptionist"));
                options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
                options.AddPolicy("AdminOrReceptionist", policy => policy.RequireRole("Admin", "Receptionist"));
                options.AddPolicy("MedicalStaff", policy => policy.RequireRole("Admin", "Doctor"));
            });

            return services;
        }
    }
}

