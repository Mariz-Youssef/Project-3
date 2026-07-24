using ClinicManagementSystem.backend.Features.Appointments.DTOs.Responses;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Appointments.Interfaces
{
    /// <summary>
    /// Provides appointment-specific data access operations.
    /// </summary>
    public interface IAppointmentRepository:IGenericRepository<Appointment>
    {
        /// <summary>
        /// Retrieves appointment details by identifier.
        /// </summary>
        /// <param name="id">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Appointment details if found; otherwise <c>null</c>.
        /// </returns>
        Task<Appointment?> GetDetailsAsync(int id, CancellationToken cancellationToken = default);

       

        /// <summary>
        /// Returns a query containing appointments with all related entities
        /// required for listing and details screens.
        /// </summary>
        /// <returns>
        /// Queryable appointment collection including navigation properties.
        /// </returns>
        IQueryable<Appointment> QueryWithDetails();

        /// <summary>
        /// Retrieves an appointment with its related doctor, patient,
        /// doctor user, patient user and department.
        /// </summary>
        /// <param name="appointmentId">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The appointment if found; otherwise <see langword="null"/>.
        /// </returns>
        Task<Appointment?> GetByIdWithDetailsAsync(int appointmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Determines whether the doctor already has an appointment
        /// that overlaps the specified time period.
        /// </summary>
        /// <param name="doctorId">
        /// Doctor identifier.
        /// </param>
        /// <param name="appointmentDate">
        /// Appointment date.
        /// </param>
        /// <param name="startTime">
        /// Appointment start time.
        /// </param>
        /// <param name="endTime">
        /// Appointment end time.
        /// </param>
        /// <param name="excludeAppointmentId">
        /// Appointment to exclude during update.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if an overlapping appointment exists;
        /// otherwise <see langword="false"/>.
        /// </returns>
        Task<bool> DoctorHasOverlappingAppointmentAsync(int doctorId, DateOnly appointmentDate, TimeOnly startTime, TimeOnly endTime, int? excludeAppointmentId = null, CancellationToken cancellationToken = default);



        /// <summary>
        /// Determines whether the patient already has an appointment
        /// that overlaps the specified time period.
        /// </summary>
        /// <param name="patientId">
        /// Patient identifier.
        /// </param>
        /// <param name="appointmentDate">
        /// Appointment date.
        /// </param>
        /// <param name="startTime">
        /// Appointment start time.
        /// </param>
        /// <param name="endTime">
        /// Appointment end time.
        /// </param>
        /// <param name="excludeAppointmentId">
        /// Appointment to exclude during update.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if an overlapping appointment exists;
        /// otherwise <see langword="false"/>.
        /// </returns>
        Task<bool> PatientHasOverlappingAppointmentAsync(int patientId, DateOnly appointmentDate, TimeOnly startTime, TimeOnly endTime, int? excludeAppointmentId = null,
            CancellationToken cancellationToken = default);


    }
}
