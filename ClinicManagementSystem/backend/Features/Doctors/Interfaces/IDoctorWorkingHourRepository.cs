using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Doctors.Interfaces
{
    public interface IDoctorWorkingHourRepository : IGenericRepository<DoctorWorkingHour>
    {
        /// <summary>
        /// Returns all working hours for a doctor.
        /// </summary>
        IQueryable<DoctorWorkingHour> GetByDoctor(int doctorId);

        /// <summary>
        /// Returns a working hour by its identifier including doctor details.
        /// </summary>
        Task<DoctorWorkingHour?> GetByIdWithDetailsAsync(int id,CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether the doctor already has working hours for the specified day.
        /// </summary>
        Task<bool> ExistsForDayAsync(int doctorId,DayOfWeek day, int? excludeId = null, CancellationToken cancellationToken = default);


        /// <summary>
        /// Retrieves the doctor's working schedule for the specified day.
        /// </summary>
        Task<DoctorWorkingHour?> GetWorkingHoursAsync(int doctorId, DayOfWeek dayOfWeek, CancellationToken cancellationToken = default);



    }
}
