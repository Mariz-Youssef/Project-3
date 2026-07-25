using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Responses;

namespace ClinicManagementSystem.backend.Features.Prescriptions.Interfaces
{
    /// <summary>
    /// Provides business operations for managing prescriptions.
    /// </summary>
    public interface IPrescriptionsService
    {
        /// <summary>
        /// Creates a new prescription.
        /// </summary>
        /// <param name="request">
        /// Prescription information.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Newly created prescription.
        /// </returns>
        Task<PrescriptionResponseDto> CreateAsync(CreatePrescriptionRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing prescription.
        /// </summary>
        /// <param name="prescriptionId">
        /// Prescription identifier.
        /// </param>
        /// <param name="request">
        /// Updated prescription information.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Updated prescription.
        /// </returns>
        Task<PrescriptionResponseDto> UpdateAsync(int prescriptionId, UpdatePrescriptionRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a prescription by its identifier.
        /// </summary>
        /// <param name="prescriptionId">
        /// Prescription identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Prescription details.
        /// </returns>
        Task<PrescriptionResponseDto> GetByIdAsync(int prescriptionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves paginated prescriptions.
        /// </summary>
        /// <param name="pagination">
        /// Pagination parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Paginated prescriptions.
        /// </returns>
        Task<PagedResult<PrescriptionResponseDto>> GetAllAsync(PaginationParameters pagination, CancellationToken cancellationToken = default);


        /// <summary>
        /// Deletes a prescription.
        /// </summary>
        /// <param name="prescriptionId">
        /// Prescription identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        Task DeleteAsync(int prescriptionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether a prescription exists.
        /// </summary>
        /// <param name="prescriptionId">
        /// Prescription identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the prescription exists;
        /// otherwise <see langword="false"/>.
        /// </returns>
        Task<bool> ExistsAsync(int prescriptionId, CancellationToken cancellationToken = default);



    }
}
