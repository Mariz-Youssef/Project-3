using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Prescriptions.Interfaces
{
    /// <summary>
    /// Provides prescription-specific data access operations.
    /// </summary>
    public interface IPrescriptionsRepository: IGenericRepository<Prescription>
    {
        /// <summary>
        /// Retrieves a prescription together with all related entities.
        /// </summary>
        /// <param name="prescriptionId">
        /// Prescription identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The prescription if found; otherwise <see langword="null"/>.
        /// </returns>
        Task<Prescription?> GetByIdWithDetailsAsync(int prescriptionId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Returns a query including all related entities
        /// required by the Prescription feature.
        /// </summary>
        /// <returns>
        /// Queryable prescription collection.
        /// </returns>
        IQueryable<Prescription> QueryWithDetails();

        /// <summary>
        /// Determines whether the specified medical record
        /// contains at least one prescription.
        /// </summary>
        /// <param name="medicalRecordId">
        /// Medical record identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if at least one prescription exists;
        /// otherwise <see langword="false"/>.
        /// </returns>
        Task<bool> ExistsForMedicalRecordAsync(int medicalRecordId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a paginated collection of all prescriptions.
        /// </summary>
        /// <param name="pagination">
        /// Pagination parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Paginated prescription collection.
        /// </returns>
        Task<PagedResult<Prescription>> GetAllPagedAsync(PaginationParameters pagination, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a paginated collection of prescriptions
        /// belonging to the specified doctor.
        /// </summary>
        /// <param name="doctorId">
        /// Doctor identifier.
        /// </param>
        /// <param name="pagination">
        /// Pagination parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Paginated prescription collection.
        /// </returns>
        Task<PagedResult<Prescription>> GetDoctorPrescriptionsPagedAsync(int doctorId, PaginationParameters pagination, CancellationToken cancellationToken = default);


        /// <summary>
        /// Returns a paginated collection of prescriptions
        /// belonging to the specified patient.
        /// </summary>
        /// <param name="patientId">
        /// Patient identifier.
        /// </param>
        /// <param name="pagination">
        /// Pagination parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Paginated prescription collection.
        /// </returns>
        Task<PagedResult<Prescription>> GetPatientPrescriptionsPagedAsync(int patientId, PaginationParameters pagination, CancellationToken cancellationToken = default);




    }
}
