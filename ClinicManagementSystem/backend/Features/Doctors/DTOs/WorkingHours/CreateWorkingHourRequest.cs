namespace ClinicManagementSystem.backend.Features.Doctors.DTOs.WorkingHours
{
    public class CreateWorkingHourRequest
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
