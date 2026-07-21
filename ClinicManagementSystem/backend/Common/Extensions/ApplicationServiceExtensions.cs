using AutoMapper;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Features.Patients.Repositories;
using ClinicManagementSystem.backend.Features.Patients.Services;

namespace ClinicManagementSystem.backend.Common.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IPatientService, PatientService>();

        return services;
    }
}
