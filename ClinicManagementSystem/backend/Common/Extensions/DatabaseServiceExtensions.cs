using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Common.Extensions
{

    /// <summary>
    /// Extension methods for registering the application's database.
    /// </summary>
    public static class DatabaseServiceExtensions
    {
        /// <summary>
        /// Registers the SQL Server database context.
        /// </summary>
        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IApplicationDbContext>(provider =>
                 provider.GetRequiredService<ApplicationDbContext>());


            return services;
        }
    }
}
