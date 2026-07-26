using ClinicManagementSystem.backend.Features.Doctors.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateDoctorValidator>();

            return services;
        }
    }
}
