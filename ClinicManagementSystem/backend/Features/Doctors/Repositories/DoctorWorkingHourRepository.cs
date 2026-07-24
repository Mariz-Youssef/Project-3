using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Doctors.Repositories
{
    public class DoctorWorkingHourRepository : GenericRepository<DoctorWorkingHour>, IDoctorWorkingHourRepository
    {
        public DoctorWorkingHourRepository(ApplicationDbContext context): base(context)
        {
        }

        public IQueryable<DoctorWorkingHour> GetByDoctor(int doctorId)
        {
            return Query().Include(x => x.Doctor).Where(x => x.DoctorId == doctorId);
        }

        public async Task<DoctorWorkingHour?> GetByIdWithDetailsAsync(int id,CancellationToken cancellationToken = default)
        {
            return await Query()
                .Include(x => x.Doctor)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsForDayAsync(int doctorId,DayOfWeek day,int? excludeId = null,CancellationToken cancellationToken = default)
        {
            return await Query().AnyAsync(x =>
                x.DoctorId == doctorId &&
                x.DayOfWeek == day &&
                (!excludeId.HasValue || x.Id != excludeId.Value),cancellationToken);
        }
    }
}
