namespace ClinicManagementSystem.backend.Features.Authentication.DTOs.Responses
{
    /// <summary>
    /// Response returned after an Admin successfully creates a Doctor's login identity.
    /// </summary>
    public class DoctorAccountResponseDto
    {
        /// <summary>Gets or sets the newly created user's Id.</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the doctor's full name.</summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>Gets or sets the doctor's email.</summary>
        public string Email { get; set; } = string.Empty;

    }
}
