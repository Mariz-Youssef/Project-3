namespace ClinicManagementSystem.backend.Features.Doctors.DTOs.Leaves
{
    public class UpdateLeaveRequest
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Reason { get; set; }
    }
}
