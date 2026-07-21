using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Requests;
using ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Responses;

namespace ClinicManagementSystem.backend.Features.DepartmentFeature.Interfaces
{
    /// <summary>
    /// Provides department business operations.
    /// </summary>
    public interface IDepartmentService
    {

        /// <summary>
        /// Retrieves all departments with pagination.
        /// </summary>
        Task<PagedResult<DepartmentResponseDto>> GetAllAsync(PaginationParameters pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a department by its identifier.
        /// </summary>
        Task<DepartmentDetailsResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new department.
        /// </summary>
        Task<DepartmentResponseDto> CreateAsync(CreateDepartmentRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        Task<DepartmentResponseDto> UpdateAsync(int id, UpdateDepartmentRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a department.
        /// </summary>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether a department exists.
        /// </summary>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches departments by name with pagination.
        /// </summary>
        Task<PagedResult<DepartmentResponseDto>> SearchAsync(string searchTerm, PaginationParameters pagination, CancellationToken cancellationToken = default);


        

    }
}
