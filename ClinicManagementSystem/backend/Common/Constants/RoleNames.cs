namespace ClinicManagementSystem.backend.Common.Constants
{
    public class RoleNames
    {
        /// <summary>The system administrator role. Full access to all resources.</summary>
        public const string Admin = "Admin";

        /// <summary>
        /// The doctor role. Manages their own working hours, leave,
        /// appointments, and medical records.
        /// </summary>
        public const string Doctor = "Doctor";

        /// <summary>
        /// The patient role. Books appointments with doctors based on
        /// available slots.
        /// </summary>
        public const string Patient = "Patient";

        /// <summary>
        /// All role names in the system, used by seeding.
        /// </summary>
        public static readonly IReadOnlyList<string> All = new[] { Admin, Doctor, Patient };
    }
}
