using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Features.Doctors.Repositories;
using ClinicManagementSystem.backend.Features.Doctors.Services;

namespace ClinicManagementSystem.backend.Features.Doctors.DependencyInjection
{
    public static class DoctorExtensions
    {
        public static IServiceCollection AddDoctorFeature(this IServiceCollection services)
        {
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IDoctorService, DoctorService>();

            return services;
        }
    }
}
