using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;
using System.Threading;

namespace ClinicManagementSystem.backend.Features.Doctors.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        IQueryable<Doctor> GetAllWithDetails();
        IQueryable<Doctor> GetByDepartment(int departmentId);
        IQueryable<Doctor> GetBySpecialization(string specialization);
        Task<Doctor?> GetByIdWithDetailsAsync(int id,CancellationToken cancellationToken = default);
        Task<bool> LicenseExistsAsync(string licenseNumber,CancellationToken cancellationToken = default);
        Task<bool> UserAlreadyAssignedAsync(int userId,CancellationToken cancellationToken = default);
    }
}
