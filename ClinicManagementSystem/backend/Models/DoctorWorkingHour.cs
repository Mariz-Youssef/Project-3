namespace ClinicManagementSystem.backend.Models
{
    /// <summary>
    /// Represents a doctor's recurring weekly working schedule.
    /// Each record defines one working day and its working hours.
    /// </summary>
    public class DoctorWorkingHour:BaseEntity
    {
        /// <summary>
        /// Gets or sets the doctor identifier.
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Gets or sets the day of the week the doctor works.
        /// Uses the built-in System.DayOfWeek enum.
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// Gets or sets the doctor's work start time.
        /// </summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// Gets or sets the doctor's work end time.
        /// </summary>
        public TimeOnly EndTime { get; set; }

        // ============================
        // Navigation Properties
        // ============================

        /// <summary>
        /// Gets the doctor who owns this schedule.
        /// Represents a many-to-one relationship.
        /// </summary>
        public Doctor Doctor { get; set; } = null!;
    }
}
