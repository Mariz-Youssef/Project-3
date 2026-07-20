using ClinicManagementSystem.backend.Data;
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

            return services;
        }
    }
}
