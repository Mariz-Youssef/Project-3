using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Persistence.Interfaces;
using ClinicManagementSystem.backend.Persistence.Repositories;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    /// <summary>
    /// Contains extension methods for registering application services and repositories.
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        /// <summary>
        /// Registers application services and repositories.
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // SaveChanges abstraction
            services.AddScoped<ISaveChanges, ApplicationDbContext>();

            // Register your application services here
            // services.AddScoped<IDepartmentService, DepartmentService>();
            // services.AddScoped<IDoctorService, DoctorService>();
            // services.AddScoped<IPatientService, PatientService>();

            return services;
        }

    }
}
