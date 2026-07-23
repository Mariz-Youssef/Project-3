using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;

/// <summary>
/// Provides medical record business operations.
/// </summary>
public interface IMedicalRecordService
{
    /// <summary>
    /// Creates a medical record.
    /// </summary>
    Task<MedicalRecordResponse> CreateAsync(int doctorUserId, CreateMedicalRecordRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets medical record details by identifier after access checks.
    /// </summary>
    Task<MedicalRecordResponse> GetByIdAsync(int medicalRecordId, int currentUserId, bool isAdmin, bool isDoctor, bool isPatient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing medical record.
    /// </summary>
    Task<MedicalRecordResponse> UpdateAsync(int medicalRecordId, int doctorUserId, UpdateMedicalRecordRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a patient's medical history after access checks.
    /// </summary>
    Task<PagedResult<MedicalHistoryResponse>> GetPatientHistoryAsync(int patientId, PaginationParameters pagination, int currentUserId, bool isAdmin, bool isDoctor, bool isPatient, CancellationToken cancellationToken = default);
}
