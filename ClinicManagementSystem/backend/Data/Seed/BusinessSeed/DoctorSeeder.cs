using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Data.Seed.BusinessSeed
{
    /// <summary>
    /// Seeds doctor profiles.
    /// </summary>
    public static class DoctorSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Prevent duplicate seeding.
            if (await context.Doctors.AnyAsync())
                return;

            // Load doctor user accounts.
            var users = await context.Users
                .ToDictionaryAsync(user => user.Email!);

            // Load departments.
            var departments = await context.Departments
                .ToDictionaryAsync(department => department.Name);

            var doctors = new List<Doctor>
        {
            new()
            {
                UserId = users["dr.ahmed@clinic.com"].Id,
                DepartmentId = departments["Cardiology"].Id,
                LicenseNumber = "CARD-1001",
                ConsultationFee = 500,
                YearsOfExperience = 12,
                Specialization = "Consultant Cardiologist with extensive experience in heart diseases."
            },

            new()
            {
                UserId = users["dr.sara@clinic.com"].Id,
                DepartmentId = departments["Dermatology"].Id,
                LicenseNumber = "DERM-1002",
                ConsultationFee = 450,
                YearsOfExperience = 8,
                Specialization = "Specialist in skin diseases and cosmetic dermatology."
            },

            new()
            {
                UserId = users["dr.khaled@clinic.com"].Id,
                DepartmentId = departments["Orthopedics"].Id,
                LicenseNumber = "ORTH-1003",
                ConsultationFee = 600,
                YearsOfExperience = 15,
                Specialization = "Orthopedic consultant specialized in joint replacement."
            },

            new()
            {
                UserId = users["dr.nour@clinic.com"].Id,
                DepartmentId = departments["Pediatrics"].Id,
                LicenseNumber = "PED-1004",
                ConsultationFee = 400,
                YearsOfExperience = 7,
                Specialization = "Dedicated pediatrician providing healthcare for children."
            },

            new()
            {
                UserId = users["dr.omar@clinic.com"].Id,
                DepartmentId = departments["Neurology"].Id,
                LicenseNumber = "NEUR-1005",
                ConsultationFee = 650,
                YearsOfExperience = 14,
                Specialization = "Neurologist specializing in brain and nervous system disorders."
            }
        };

            await context.Doctors.AddRangeAsync(doctors);

            await context.SaveChangesAsync();
        }
    }
}
