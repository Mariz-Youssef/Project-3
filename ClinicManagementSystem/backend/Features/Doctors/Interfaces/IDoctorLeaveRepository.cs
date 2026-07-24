using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Doctors.Interfaces
{
    public interface IDoctorLeaveRepository : IGenericRepository<DoctorLeave>
    {
        /// <summary>
        /// Returns all leaves for a doctor.
        /// </summary>
        IQueryable<DoctorLeave> GetByDoctor(int doctorId);

        /// <summary>
        /// Returns a leave including doctor details.
        /// </summary>
        Task<DoctorLeave?> GetByIdWithDetailsAsync(int id,CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether a leave overlaps an existing leave.
        /// </summary>
        Task<bool> HasOverlappingLeaveAsync(int doctorId,DateOnly start,DateOnly end, int? excludeId ,CancellationToken cancellationToken = default);
    }
}
