using ClinicManagementSystem.backend.Common.Services;
using ClinicManagementSystem.backend.Common.Services.Interfaces;
using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.Appointments.Interfaces;
using ClinicManagementSystem.backend.Features.Appointments.Repositories;
using ClinicManagementSystem.backend.Features.Appointments.Services;
using ClinicManagementSystem.backend.Features.Authentication.Extensions;
using ClinicManagementSystem.backend.Features.DepartmentFeature;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Interfaces;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Repositories;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Services;
using ClinicManagementSystem.backend.Features.Doctors.DependencyInjection;
using ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;
using ClinicManagementSystem.backend.Features.MedicalRecords.Repositories;
using ClinicManagementSystem.backend.Features.MedicalRecords.Services;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Features.Patients.Repositories;
using ClinicManagementSystem.backend.Features.Patients.Services;
using ClinicManagementSystem.backend.Features.Prescriptions.Interfaces;
using ClinicManagementSystem.backend.Features.Prescriptions.Repositories;
using ClinicManagementSystem.backend.Features.Prescriptions.Services;
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
            services.AddMapping();
            services.AddValidation();

            // Register your application services here

            services.AddAuthFeatureServices();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddAutoMapper(_ => { }, AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            services.AddScoped<IMedicalRecordsService, MedicalRecordsService>();
            services.AddScoped<IPrescriptionsRepository, PrescriptionsRepository>();
            services.AddScoped<IPrescriptionsService, PrescriptionsService>();
            services.AddDoctorFeature();

            //Get AuthenticatedUser info
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();


            return services;
        }

    }
}
