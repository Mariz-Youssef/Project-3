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

        public IQueryable<Doctor> GetAllWithDetails()
        {
            return Query()
                .Include(d => d.User)
                .Include(d => d.Department);
        }

        public IQueryable<Doctor> GetByDepartment(int departmentId)
        {
            return Query()
                .Include(d => d.User)
                .Include(d => d.Department)
                .Where(d => d.DepartmentId == departmentId);
        }


        public IQueryable<Doctor> GetBySpecialization(string specialization)
        {
            return Query()
                .Include(d => d.User)
                .Include(d => d.Department)
                .Where(d => d.Specialization == specialization);
        }


        public async Task<bool> LicenseExistsAsync(string licenseNumber, CancellationToken cancellationToken = default)
        {
            return await Query().AnyAsync(d=>d.LicenseNumber== licenseNumber, cancellationToken);
        }

        public async Task<bool> UserAlreadyAssignedAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await Query().AnyAsync(d=>d.UserId== userId, cancellationToken);
        }
        public async Task<Doctor?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await Query()
                .Include(d => d.User)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<Doctor?> GetDoctorForAppointmentAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            return await _context.Doctors
                .Include(d => d.User)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(
                    d => d.Id == doctorId,
                    cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Doctor?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await Query()
                .FirstOrDefaultAsync(doctor => doctor.UserId == userId, cancellationToken);
        }
    }
}
