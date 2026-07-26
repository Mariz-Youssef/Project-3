using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Responses;
using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Doctors.Interfaces
{
    public interface IDoctorService
    {
        /// <summary>
        /// Retrieves all doctors.
        /// </summary>
        Task<PagedResult<DoctorResponse>> GetAllAsync( PaginationParameters pagination, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves a doctor by identifier.
        /// </summary>
        Task<DoctorResponse?> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        /// <summary>
        /// Creates a new doctor.
        /// </summary>
        Task<DoctorResponse> CreateAsync(CreateDoctorRequest request,CancellationToken cancellationToken = default);
        /// <summary>
        /// Update doctor properties.
        /// </summary>
        Task<DoctorResponse> UpdateAsync(int id, UpdateDoctorRequest request,CancellationToken cancellationToken = default);
        /// <summary>
        /// Delete doctor.
        /// </summary>
        Task DeleteAsync(int id,CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all doctors belonging to the specified department.
        /// </summary>
        /// <param name="departmentId">The unique identifier of the department.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A read-only list of doctors in the specified department.</returns>
        Task<PagedResult<DoctorResponse>> GetByDepartmentAsync(int departmentId, PaginationParameters pagination,CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieves all doctors with the specified specialization.
        /// </summary>
        /// <param name="specialization">The doctor's medical specialization.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A read-only list of doctors matching the specified specialization.</returns>
        Task<PagedResult<DoctorResponse>> GetBySpecializationAsync(string specialization, PaginationParameters pagination, CancellationToken cancellationToken = default);
    }
}
