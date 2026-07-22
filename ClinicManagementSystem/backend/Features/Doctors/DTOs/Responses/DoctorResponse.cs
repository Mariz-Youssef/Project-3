namespace ClinicManagementSystem.backend.Features.Doctors.DTOs.Responses
{
    public class DoctorResponse
    {

        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
        public decimal ConsultationFee { get; set; }
    }
}
