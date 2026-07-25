using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Requests;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Responses;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces
{
    /// <summary>
    /// Provides business operations for managing medical records.
    /// </summary>
    public interface IMedicalRecordsService
    {
        /// <summary>
        /// Creates a new medical record.
        /// </summary>
        Task<MedicalRecordResponseDto> CreateAsync(CreateMedicalRecordRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing medical record.
        /// </summary>
        Task<MedicalRecordResponseDto> UpdateAsync(int medicalRecordId, UpdateMedicalRecordRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a medical record by identifier.
        /// </summary>
        Task<MedicalRecordResponseDto> GetByIdAsync(int medicalRecordId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves paginated medical records.
        /// </summary>
        Task<PagedResult<MedicalRecordResponseDto>> GetAllAsync(PaginationParameters pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a medical record.
        /// </summary>
        Task DeleteAsync(int medicalRecordId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether a medical record exists.
        /// </summary>
        Task<bool> ExistsAsync(int medicalRecordId, CancellationToken cancellationToken = default);


    }
}
