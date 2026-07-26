namespace ClinicManagementSystem.backend.Features.Doctors.DTOs.Leaves
{
    public class LeaveResponse
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Reason { get; set; }
    }
}
