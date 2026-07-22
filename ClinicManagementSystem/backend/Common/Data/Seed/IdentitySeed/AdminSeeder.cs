using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.backend.Common.Data.Seed.IdentitySeed
{
    /// <summary>
    /// Seeds the default system administrator.
    /// </summary>
    public static class AdminSeeder
    {
        /// <summary>
        /// Creates the default administrator account.
        /// </summary>
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            const string email = "admin@clinic.com";

            // Check if the administrator already exists.
            if (await userManager.FindByEmailAsync(email) is not null)
                return;

            var admin = new ApplicationUser
            {
                FullName = "System Administrator",
                UserName = "admin",
                Email = email,
                EmailConfirmed = true,
                PhoneNumber = "0110987654"
            };

            IdentityResult result = await userManager.CreateAsync(admin, "Admin@123");

            if (!result.Succeeded)
            {
                throw new Exception(
                    $"Failed to create administrator: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            await userManager.AddToRoleAsync(admin, RoleNames.Admin);
        }
    }
}
