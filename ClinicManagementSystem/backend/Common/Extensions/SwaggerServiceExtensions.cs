using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    /// <summary>
    /// Extension methods for registering Swagger/OpenAPI, including
    /// the JWT Bearer "Authorize" button used to test protected endpoints.
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// Registers Swagger generation with a Bearer token security definition,
        /// so protected endpoints can be tested directly from the Swagger UI.
        /// </summary>
        /// <param name="services">The service collection to add to.</param>
        /// <returns>The same service collection, for chaining.</returns>
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Clinic Management System API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,       // changed from ApiKey
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT access token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.SupportNonNullableReferenceTypes();

            });

            return services;
        }
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




