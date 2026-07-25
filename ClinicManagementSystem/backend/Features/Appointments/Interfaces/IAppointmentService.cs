using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Appointments.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Appointments.DTOs.Responses;

namespace ClinicManagementSystem.backend.Features.Appointments.Interfaces
{
    /// <summary>
    /// Provides appointment business operations.
    /// </summary>
    public interface IAppointmentService
    {
        /// <summary>
        /// Books a new appointment.
        /// </summary>
        Task<AppointmentResponseDto> CreateAsync(CreateAppointmentRequestDto request, CancellationToken cancellationToken = default);


        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        Task<AppointmentResponseDto> UpdateAsync(int appointmentId, UpdateAppointmentRequestDto request, CancellationToken cancellationToken = default);


        /// <summary>
        /// Cancels an appointment.
        /// </summary>
        Task DeleteAsync(int appointmentId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Returns appointment details.
        /// </summary>
        Task<AppointmentDetailsResponseDto> GetByIdAsync(int appointmentId, CancellationToken cancellationToken = default);


        /// <summary>
        /// Returns all appointments with pagination.
        /// </summary>
        Task<PagedResult<AppointmentResponseDto>> GetAllAsync(PaginationParameters pagination, CancellationToken cancellationToken = default);


        /// <summary>
        /// Determines whether an appointment exists.
        /// </summary>
        Task<bool> ExistsAsync(int appointmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Confirms a pending appointment.
        /// </summary>
        Task<AppointmentResponseDto> ConfirmAsync(int appointmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Completes a confirmed appointment.
        /// </summary>
        Task<AppointmentResponseDto> CompleteAsync(int appointmentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels a pending or confirmed appointment.
        /// </summary>
        Task<AppointmentResponseDto> CancelAsync(int appointmentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<AvailableSlotResponse>> GetAvailableSlotsAsync(int doctorId,DateOnly date,CancellationToken cancellationToken);

    }
}
