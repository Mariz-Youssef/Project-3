using ClinicManagementSystem.backend.Common.Exceptions;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for registering global exception handling.
    /// </summary>
    public static class ExceptionServiceExtensions
    {
        /// <summary>
        /// Registers the global exception handler.
        /// </summary>

        public static IServiceCollection AddGlobalExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
    }
}
