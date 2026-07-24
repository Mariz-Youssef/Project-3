namespace ClinicManagementSystem.backend.Features.Doctors.DTOs.Leaves
{
    public class CreateLeaveRequest
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Reason { get; set; }
    }
}
