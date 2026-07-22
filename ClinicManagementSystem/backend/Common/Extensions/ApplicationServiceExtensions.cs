using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.DepartmentFeature;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Interfaces;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Repositories;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Services;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Features.Patients.Repositories;
using ClinicManagementSystem.backend.Features.Patients.Services;
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

         

            // AutoMapper
            services.AddAutoMapper(typeof(DepartmentProfile).Assembly);

            // Register your application services here

            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddAutoMapper(_ => { }, AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientService, PatientService>();


            return services;
        }

    }
}
