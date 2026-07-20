using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.backend.Data.Seed.IdentitySeed
{
    /// <summary>
    /// Seeds the default receptionist account.
    /// </summary>
    public static class ReceptionistSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            const string email = "reception@clinic.com";

            if (await userManager.FindByEmailAsync(email) is not null)
                return;

            var receptionist = new ApplicationUser
            {
                FullName = "Clinic Receptionist",
                UserName = "reception",
                Email = email,
                EmailConfirmed = true,
                PhoneNumber = "01008795432"
            };

            var result = await userManager.CreateAsync(
                receptionist,
                "Reception@123");

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine,
                    result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(
                receptionist,
                "Receptionist");
        }
    }
}
