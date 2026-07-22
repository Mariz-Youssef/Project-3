namespace ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Responses
{
    /// <summary>
    /// Represents detailed department information.
    /// </summary>
    public class DepartmentDetailsResponseDto
    {
        /// <summary>
        /// Gets or sets the department identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the department name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the department description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets the number of doctors assigned to the department.
        /// </summary>
        public int DoctorsCount { get; set; }
    }
}
