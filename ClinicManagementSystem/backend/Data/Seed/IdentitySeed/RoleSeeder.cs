using ClinicManagementSystem.backend.Common.Constants;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.backend.Data.Seed.IdentitySeed
{
    /// <summary>
    /// Creates the default system roles if they do not exist.
    /// </summary>
    public static class RoleSeeder
    {
        /// <summary>
        /// Seeds the system roles (Admin, Doctor, Patient).
        /// </summary>
        /// <param name="roleManager">The role manager used to create roles.</param>
        public static async Task SeedAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            foreach (var role in RoleNames.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
                }
            }
        }
    }
}
