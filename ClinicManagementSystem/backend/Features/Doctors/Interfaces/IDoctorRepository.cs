using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;
using System.Threading;

namespace ClinicManagementSystem.backend.Features.Doctors.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<bool> LicenseExistsAsync(string licenseNumber, CancellationToken cancellationToken = default);
        Task<bool> UserAlreadyAssignedAsync(int userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Doctor>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default);
        Task<Doctor?> GetByIdWithDetailsAsync(int id,CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Doctor>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    }
}
