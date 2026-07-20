using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.backend.Data.Seed.IdentitySeed
{
    /// <summary>
    /// Seeds sample patient user accounts.
    /// </summary>
    public static class PatientUserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var patientUsers = new List<ApplicationUser>
        {
            new()
            {
                FullName = "Mohamed Ahmed",
                UserName = "patient1",
                Email = "patient1@clinic.com",
                EmailConfirmed = true,
                PhoneNumber = "01220000001"
            },

            new()
            {
                FullName = "Sara Ali",
                UserName = "patient2",
                Email = "patient2@clinic.com",
                EmailConfirmed = true,
                PhoneNumber = "01220000002"
            },

            new()
            {
                FullName = "Omar Hassan",
                UserName = "patient3",
                Email = "patient3@clinic.com",
                EmailConfirmed = true,
                PhoneNumber = "01220000003"
            }
        };

            foreach (var patient in patientUsers)
            {
                var existingUser = await userManager.FindByEmailAsync(patient.Email!);

                if (existingUser is not null)
                    continue;

                var result = await userManager.CreateAsync(
                    patient,
                    "Patient@123");

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create patient '{patient.FullName}': " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(patient, "Patient");
            }
        }
    }
}
