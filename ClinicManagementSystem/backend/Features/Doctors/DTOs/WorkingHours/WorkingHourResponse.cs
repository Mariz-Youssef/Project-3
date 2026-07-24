namespace ClinicManagementSystem.backend.Features.Doctors.DTOs.WorkingHours
{
    public class WorkingHourResponse
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
