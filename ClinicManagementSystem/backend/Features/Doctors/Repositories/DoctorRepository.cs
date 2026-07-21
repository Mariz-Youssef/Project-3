using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Doctors.Repositories
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context): base(context)
        {
        }

        public async Task<IReadOnlyList<Doctor>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await Query()
            .Include(d => d.User)
            .Include(d => d.Department)
            .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Doctor>> GetByDepartmentAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await Query()
            .Include(d => d.User)
            .Include(d => d.Department)
            .Where(d => d.DepartmentId == departmentId)
            .ToListAsync(cancellationToken);
        }

        public async Task<Doctor?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await Query()
            .Include(d => d.User)
            .Include(d => d.Department)
            .FirstOrDefaultAsync(d => d.Id == id,cancellationToken);
        }

        public async Task<IReadOnlyList<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
        {
            return await Query()
            .Include(d => d.User)
            .Include(d => d.Department)
            .Where(d => d.Specialization == specialization)
            .ToListAsync(cancellationToken);
        }

        public async Task<bool> LicenseExistsAsync(string licenseNumber, CancellationToken cancellationToken = default)
        {
            return await Query().AnyAsync(d=>d.LicenseNumber== licenseNumber, cancellationToken);
        }

        public async Task<bool> UserAlreadyAssignedAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await Query().AnyAsync(d=>d.UserId== userId, cancellationToken);
        }
    }
}
