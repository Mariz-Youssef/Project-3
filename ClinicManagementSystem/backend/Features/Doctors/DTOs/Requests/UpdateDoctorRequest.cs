namespace ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests
{
    public class UpdateDoctorRequest
    {
        public int DepartmentId { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
    }
}
