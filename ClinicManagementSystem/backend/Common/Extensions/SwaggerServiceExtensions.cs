using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring Swagger/OpenAPI.
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddApplicationSwagger(
           this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Clinic Management System API",
                    Version = "v1",
                    Description = "REST API for managing clinic departments, doctors, patients, appointments, and medical records."
                });

                string xmlFile =
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                string xmlPath =
                    Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);

                options.SupportNonNullableReferenceTypes();
            });

            return services;
        }

        /// <summary>
        /// Enables Swagger middleware.
        /// </summary>
        public static WebApplication UseApplicationSwagger(
            this WebApplication app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "Clinic Management System API";

                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "Clinic Management System API v1");

                options.DisplayRequestDuration();
            });

            return app;
        }
    }
}
