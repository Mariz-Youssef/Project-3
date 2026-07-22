using ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Responses;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.DepartmentFeature.Interfaces
{
    /// <summary>
    /// Provides department-specific data access operations.
    /// </summary>
    public interface IDepartmentRepository : IGenericRepository<Department>
    {

        /// <summary>
        /// Retrieves details of a department by its identifier.
        /// </summary>
        Task<DepartmentDetailsResponseDto?> GetDetailsAsync(int id, CancellationToken cancellationToken = default);
    }
}
