using ClinicManagementSystem.backend.Features.Authentication.Extensions;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    /// <summary>
    /// Aggregates service registrations from every feature into a single call.
    /// Each feature owns and maintains its own registration method; this class
    /// only wires them together so Program.cs doesn't need to reference every
    /// feature individually.
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAuthFeatureServices();
            // services.AddDoctorFeatureServices();
            // services.AddPatientFeatureServices();
            // add one line per feature as they're built

            return services;
        }
    }
}
