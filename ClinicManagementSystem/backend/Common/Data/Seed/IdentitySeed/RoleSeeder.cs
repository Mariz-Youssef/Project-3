using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.backend.Common.Data.Seed.IdentitySeed
{
    /// <summary>
    /// Creates the default system roles if they do not exist.
    /// </summary>
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            string[] roles =
            {
            "Admin",
            "Doctor",
            "Receptionist",
            "Patient"
        };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<int>
                        {
                            Name = role
                        });
                }
            }
        }
    }
}
