namespace ClinicManagementSystem.backend.Features.Doctors.DTOs.WorkingHours
{
    public class UpdateWorkingHourRequest
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
