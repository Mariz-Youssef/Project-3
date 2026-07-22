namespace ClinicManagementSystem.backend.Features.Authentication.DTOs
{
    public class RegisterResponseDto
    {
        /// <summary>Gets or sets the newly created user's Id.</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the user's full name.</summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>Gets or sets the user's email.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Gets or sets the role assigned to the user.</summary>
        public string Role { get; set; } = string.Empty;
    }
}
