using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces
{
    /// <summary>
    /// Provides medical record-specific data access operations.
    /// </summary>
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        /// <summary>
        /// Retrieves a medical record with all related entities.
        /// </summary>
        /// <param name="medicalRecordId">
        /// Medical record identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The medical record if found; otherwise <see langword="null"/>.
        /// </returns>
        Task<MedicalRecord?> GetByIdWithDetailsAsync(int medicalRecordId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all medical records with related entities.
        /// </summary>
        IQueryable<MedicalRecord> QueryWithDetails();


        /// <summary>
        /// Determines whether a medical record already exists
        /// for the specified appointment.
        /// </summary>
        /// <param name="appointmentId">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if a medical record already exists;
        /// otherwise <see langword="false"/>.
        /// </returns>
        Task<bool> ExistsByAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a paginated collection of all medical records.
        /// </summary>
        Task<PagedResult<MedicalRecord>> GetAllPagedAsync(PaginationParameters pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a paginated collection of medical records
        /// belonging to the specified doctor.
        /// </summary>
        Task<PagedResult<MedicalRecord>> GetDoctorRecordsPagedAsync(int doctorId, PaginationParameters pagination, CancellationToken cancellationToken = default);


        /// <summary>
        /// Returns a paginated collection of medical records
        /// belonging to the specified patient.
        /// </summary>
        Task<PagedResult<MedicalRecord>> GetPatientRecordsPagedAsync(int patientId, PaginationParameters pagination, CancellationToken cancellationToken = default);


    }
}
