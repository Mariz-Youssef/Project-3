using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Patients.Repositories;

/// <summary>
/// Provides patient-specific data access operations.
/// </summary>
public sealed class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PatientRepository"/> class.
    /// </summary>
    /// <param name="context">
    /// Application database context.
    /// </param>
    public PatientRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves a patient profile with its related user by user identifier.
    /// </summary>
    /// <param name="userId">
    /// User identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// The matching patient profile; otherwise <see langword="null"/>.
    /// </returns>
    public async Task<Patient?> GetPatientProfileWithUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Include(patient => patient.User)
            .FirstOrDefaultAsync(patient => patient.UserId == userId, cancellationToken);
    }

    /// <summary>
    /// Retrieves a patient profile with its related user by patient identifier.
    /// </summary>
    /// <param name="patientId">
    /// Patient identifier.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// The matching patient profile; otherwise <see langword="null"/>.
    /// </returns>
    public async Task<Patient?> GetPatientByIdWithUserAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Include(patient => patient.User)
            .FirstOrDefaultAsync(patient => patient.Id == patientId, cancellationToken);
    }

    /// <summary>
    /// Retrieves a paginated list of all patients with related users.
    /// </summary>
    /// <param name="pagination">
    /// Pagination parameters.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// A paginated patient result.
    /// </returns>
    public async Task<PagedResult<Patient>> GetAllPatientsPagedAsync(PaginationParameters pagination, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Include(patient => patient.User)
            .AsNoTracking()
            .OrderBy(patient => patient.Id)
            .ToPagedResultAsync(pagination, cancellationToken);
    }

    /// <summary>
    /// Searches patients by name or emergency phone and returns a paginated result.
    /// </summary>
    /// <param name="searchTerm">
    /// Search text.
    /// </param>
    /// <param name="pagination">
    /// Pagination parameters.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation token.
    /// </param>
    /// <returns>
    /// A paginated patient search result.
    /// </returns>
    public async Task<PagedResult<Patient>> SearchPatientsPagedAsync(string searchTerm, PaginationParameters pagination, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Include(patient => patient.User)
            .AsNoTracking()
            .Where(patient => patient.User.FullName.Contains(searchTerm) || patient.EmergencyContactPhone.Contains(searchTerm))
            .OrderBy(patient => patient.Id)
            .ToPagedResultAsync(pagination, cancellationToken);
    }
}