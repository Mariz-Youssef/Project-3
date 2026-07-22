using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Patients.Interfaces;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<Patient?> GetPatientProfileWithUserAsync(int userId, CancellationToken cancellationToken = default);

    Task<Patient?> GetPatientByIdWithUserAsync(int patientId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Patient>> SearchPatientsWithUserAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}