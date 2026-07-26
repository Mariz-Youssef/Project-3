using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Data.Seed.BusinessSeed
{
    /// <summary>
    /// Seeds working schedules for doctors.
    /// </summary>
    public static class DoctorWorkingHourSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.DoctorWorkingHours.AnyAsync())
                return;

            var doctors = await context.Doctors
                .Include(d => d.User)
                .ToDictionaryAsync(d => d.User!.Email!);

            var schedules = new List<DoctorWorkingHour>();

            // Dr. Ahmed
            AddSchedule(
                schedules,
                doctors["dr.ahmed@clinic.com"].Id,
                new TimeOnly(9, 0),
                new TimeOnly(17, 0),
                new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });

            AddSchedule(
                schedules,
                doctors["dr.ahmed@clinic.com"].Id,
                new TimeOnly(9, 0),
                new TimeOnly(15, 0),
                new[] { DayOfWeek.Friday });

            // Dr. Sara
            AddSchedule(
                schedules,
                doctors["dr.sara@clinic.com"].Id,
                new TimeOnly(10, 0),
                new TimeOnly(18, 0),
                new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });

            AddSchedule(
                schedules,
                doctors["dr.sara@clinic.com"].Id,
                new TimeOnly(10, 0),
                new TimeOnly(14, 0),
                new[] { DayOfWeek.Friday });

            // Dr. Khaled
            AddSchedule(
                schedules,
                doctors["dr.khaled@clinic.com"].Id,
                new TimeOnly(8, 0),
                new TimeOnly(16, 0),
                new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });

            AddSchedule(
                schedules,
                doctors["dr.khaled@clinic.com"].Id,
                new TimeOnly(8, 0),
                new TimeOnly(13, 0),
                new[] { DayOfWeek.Friday });

            // Dr. Nour
            AddSchedule(
                schedules,
                doctors["dr.nour@clinic.com"].Id,
                new TimeOnly(9, 0),
                new TimeOnly(17, 0),
                new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Friday });

            // Dr. Omar
            AddSchedule(
                schedules,
                doctors["dr.omar@clinic.com"].Id,
                new TimeOnly(11, 0),
                new TimeOnly(19, 0),
                new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday });

            AddSchedule(
                schedules,
                doctors["dr.omar@clinic.com"].Id,
                new TimeOnly(11, 0),
                new TimeOnly(16, 0),
                new[] { DayOfWeek.Friday });

            await context.DoctorWorkingHours.AddRangeAsync(schedules);

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds working hours for one or more days.
        /// </summary>
        private static void AddSchedule(
            List<DoctorWorkingHour> schedules,
            int doctorId,
            TimeOnly start,
            TimeOnly end,
            DayOfWeek[] days)
        {
            foreach (var day in days)
            {
                schedules.Add(new DoctorWorkingHour
                {
                    DoctorId = doctorId,
                    DayOfWeek = day,
                    StartTime = start,
                    EndTime = end
                });
            }
        }
    }
}
