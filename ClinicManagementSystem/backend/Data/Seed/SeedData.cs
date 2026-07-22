using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Data.Seed.BusinessSeed;
using ClinicManagementSystem.backend.Data.Seed.IdentitySeed;
using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Data.Seed
{
    /// <summary>
    /// Initializes and seeds the application's database.
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Applies migrations and seeds initial data.
        /// </summary>
        public static async Task InitializeAsync(WebApplication app)
        {
            using IServiceScope scope = app.Services.CreateScope();

            IServiceProvider services = scope.ServiceProvider;

            var context =
                services.GetRequiredService<ApplicationDbContext>();

            var roleManager =
                services.GetRequiredService<RoleManager<IdentityRole<int>>>();

            var userManager =
                services.GetRequiredService<UserManager<ApplicationUser>>();

            // Apply pending migrations.
            await context.Database.MigrateAsync();

            // ==============================
            // Identity Seed
            // ==============================

            await RoleSeeder.SeedAsync(roleManager);

            await AdminSeeder.SeedAsync(userManager);

            //await ReceptionistSeeder.SeedAsync(userManager);

            await DoctorUserSeeder.SeedAsync(userManager);

            await PatientUserSeeder.SeedAsync(userManager);

            // ==============================
            // Business Seed
            // ==============================

            await DepartmentSeeder.SeedAsync(context);

            await DoctorSeeder.SeedAsync(context);

            await PatientSeeder.SeedAsync(context);

            await DoctorWorkingHourSeeder.SeedAsync(context);
        }
    }
}
