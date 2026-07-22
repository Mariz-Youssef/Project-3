using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Enums;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Data.Seed.BusinessSeed
{
    /// <summary>
    /// Seeds sample patients.
    /// </summary>
    public static class PatientSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Patients.AnyAsync())
                return;

            var users = await context.Users
                .ToDictionaryAsync(user => user.Email!);

            var patients = new List<Patient>
        {
            new()
            {
                UserId = users["patient1@clinic.com"].Id,
                DateOfBirth = new DateOnly(1998, 5, 10),
                Gender = Gender.Male,
                BloodGroup = BloodGroup.APositive,
                Address = "Nasr City, Cairo",
                Allergies = "Penicillin",
                EmergencyContactName = "Ahmed Mohamed",
                EmergencyContactPhone = "01090000001"
            },

            new()
            {
                UserId = users["patient2@clinic.com"].Id,
                DateOfBirth = new DateOnly(2000, 11, 18),
                Gender = Gender.Female,
                BloodGroup = BloodGroup.ONegative,
                Address = "Heliopolis, Cairo",
                Allergies = "None",
                EmergencyContactName = "Ali Hassan",
                EmergencyContactPhone = "01090000002"
            },

            new()
            {
                UserId = users["patient3@clinic.com"].Id,
                DateOfBirth = new DateOnly(1995, 2, 22),
                Gender = Gender.Male,
                BloodGroup = BloodGroup.BPositive,
                Address = "Maadi, Cairo",
                Allergies = "Seafood",
                EmergencyContactName = "Hassan Omar",
                EmergencyContactPhone = "01090000003"
            }
        };

            await context.Patients.AddRangeAsync(patients);

            await context.SaveChangesAsync();
        }
    }
}
