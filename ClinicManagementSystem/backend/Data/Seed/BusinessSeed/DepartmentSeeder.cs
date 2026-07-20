using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Data.Seed.BusinessSeed
{
    /// <summary>
    /// Seeds clinic departments.
    /// </summary>
    public static class DepartmentSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (context.Departments.Any())
                return;

            var departments = new List<Department>
        {
            new()
            {
                Name = "Cardiology",
                Description = "Heart and cardiovascular diseases."
            },

            new()
            {
                Name = "Dentistry",
                Description = "Dental care and oral surgery."
            },

            new()
            {
                Name = "Dermatology",
                Description = "Skin diseases and cosmetic treatments."
            },

            new()
            {
                Name = "Neurology",
                Description = "Brain and nervous system."
            },

            new()
            {
                Name = "Orthopedics",
                Description = "Bones, joints and muscles."
            },

            new()
            {
                Name = "Pediatrics",
                Description = "Children healthcare."
            },

            new()
            {
                Name = "Ophthalmology",
                Description = "Eye diseases and vision."
            },

            new()
            {
                Name = "ENT",
                Description = "Ear, Nose and Throat."
            }
        };

            await context.Departments.AddRangeAsync(departments);

            await context.SaveChangesAsync();
        }
    }
}
