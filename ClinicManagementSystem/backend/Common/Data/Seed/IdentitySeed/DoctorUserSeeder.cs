using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.backend.Common.Data.Seed.IdentitySeed
{
    /// <summary>
    /// This class only creates ApplicationUser records with the Doctor role.
    /// Seeds default doctor user accounts.
    /// </summary>
    public static class DoctorUserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var doctorUsers = new List<ApplicationUser>
        {
            new()
            {
                FullName = "Dr. Ahmed Hassan",
                UserName = "dr.ahmed",
                Email = "dr.ahmed@clinic.com",
                EmailConfirmed = true,
                PhoneNumber = "01010000001"
            },

            new()
            {
                FullName = "Dr. Sara Mohamed",
                UserName = "dr.sara",
                Email = "dr.sara@clinic.com",
                EmailConfirmed = true,
                PhoneNumber = "01010000002"
            },

            new()
            {
                FullName = "Dr. Khaled Ali",
                UserName = "dr.khaled",
                Email = "dr.khaled@clinic.com",
                EmailConfirmed = true,
                PhoneNumber = "01010000003"
            },

            new()
            {
                FullName = "Dr. Nour Mahmoud",
                UserName = "dr.nour",
                Email = "dr.nour@clinic.com",
                EmailConfirmed = true,
                PhoneNumber = "01010000004"
            },

            new()
            {
                FullName = "Dr. Omar Ibrahim",
                UserName = "dr.omar",
                Email = "dr.omar@clinic.com",
                EmailConfirmed = true,
                PhoneNumber = "01010000005"
            }
        };

            foreach (var doctor in doctorUsers)
            {
                var existingUser = await userManager.FindByEmailAsync(doctor.Email!);

                if (existingUser is not null)
                    continue;

                var result = await userManager.CreateAsync(
                    doctor,
                    "Doctor@123");

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create doctor user '{doctor.FullName}': " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(
                    doctor,
                    "Doctor");
            }
        }
    }
}
