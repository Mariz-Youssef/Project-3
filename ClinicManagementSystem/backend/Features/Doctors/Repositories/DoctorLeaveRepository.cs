using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Doctors.Repositories
{
    public class DoctorLeaveRepository : GenericRepository<DoctorLeave>, IDoctorLeaveRepository
    {
        public DoctorLeaveRepository(ApplicationDbContext context): base(context)
        {
        }

        public IQueryable<DoctorLeave> GetByDoctor(int doctorId)
        {
            return Query()
                .Include(x => x.Doctor)
                .Where(x => x.DoctorId == doctorId);
        }

        public async Task<DoctorLeave?> GetByIdWithDetailsAsync(int id,CancellationToken cancellationToken = default)
        {
            return await Query()
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> HasOverlappingLeaveAsync(int doctorId,DateOnly start,DateOnly end,int? excludeId = null,CancellationToken cancellationToken = default)
        {
            var query = Query().Where(l => l.DoctorId == doctorId);

            if (excludeId.HasValue)
            {
                query = query.Where(l => l.Id != excludeId.Value);
            }

            return await query.AnyAsync(l => l.StartDate <= end && l.EndDate >= start,cancellationToken);
        }

        public async Task<bool> IsOnLeaveAsync(int doctorId, DateOnly appointmentDate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(
                leave =>
                    leave.DoctorId == doctorId &&
                    leave.StartDate <= appointmentDate &&
                    leave.EndDate >= appointmentDate,
                cancellationToken);
        }
    }
}
