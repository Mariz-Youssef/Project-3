using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;

/// <summary>
/// Provides medical-record-specific data access operations.
/// </summary>
public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
{
    /// <summary>
    /// Gets a medical record by appointment identifier.
    /// </summary>
    Task<MedicalRecord?> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated medical history for a patient.
    /// </summary>
    Task<PagedResult<MedicalRecord>> GetPatientHistoryAsync(int patientId, PaginationParameters pagination, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets paginated medical history for a patient scoped to a doctor.
    /// </summary>
    Task<PagedResult<MedicalRecord>> GetPatientHistoryByDoctorAsync(int patientId, int doctorId, PaginationParameters pagination, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a medical record by identifier including appointment/doctor/patient details.
    /// </summary>
    Task<MedicalRecord?> GetByIdWithDetailsAsync(int medicalRecordId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a tracked medical record entity by identifier for update flows.
    /// </summary>
    Task<MedicalRecord?> GetByIdForUpdateAsync(int medicalRecordId, CancellationToken cancellationToken = default);
}
