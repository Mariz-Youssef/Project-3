using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Requests
{
    /// <summary>
    /// Represents the request used to update a department.
    /// </summary>
    public class UpdateDepartmentRequestDto
    {
        /// <summary>
        /// Gets or sets the department name.
        /// </summary>
        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the department description.
        /// </summary>
        [MaxLength(500, ErrorMessage = "Department description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
