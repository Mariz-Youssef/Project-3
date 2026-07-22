using Microsoft.OpenApi.Models;

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

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }
    }
        
    
    
}




