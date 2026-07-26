using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Patients.Interfaces;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<Patient?> GetPatientProfileWithUserAsync(int userId, CancellationToken cancellationToken = default);

    Task<Patient?> GetPatientByIdWithUserAsync(int patientId, CancellationToken cancellationToken = default);

    Task<PagedResult<Patient>> GetAllPatientsPagedAsync(PaginationParameters pagination, CancellationToken cancellationToken = default);

    Task<PagedResult<Patient>> SearchPatientsPagedAsync(string searchTerm, PaginationParameters pagination, CancellationToken cancellationToken = default);


    // Retrieves a patient by its related application user identifier.
    Task<Patient?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);


}