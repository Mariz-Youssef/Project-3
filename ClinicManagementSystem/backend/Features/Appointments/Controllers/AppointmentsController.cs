using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.Appointments.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Appointments.DTOs.Responses;
using ClinicManagementSystem.backend.Features.Appointments.Interfaces;
using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Appointments.Controllers
{
    /// <summary>
    /// Provides endpoints for managing clinic appointments.
    /// </summary>
    /// <remarks>
    /// This controller allows patients to book and manage their appointments,
    /// while administrators manage all appointments in the system.
    /// Business validation is delegated to <see cref="IAppointmentService"/>.
    /// </remarks>
    
    [Route("api/Appointments")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentsController"/> class.
        /// </summary>
        /// <param name="appointmentService">
        /// Provides appointment business operations.
        /// </param>
        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }



        /// <summary>
        /// Retrieves all appointments using pagination.
        /// </summary>
        /// <param name="pagination">
        /// Pagination parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// A paginated list of appointments.
        /// </returns>
        /// <response code="200">
        /// Appointments retrieved successfully.
        /// </response>
        /// <response code="401">
        /// User is not authenticated.
        /// </response>
        /// <response code="403">
        /// User is not authorized to view appointments.
        /// </response>

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<AppointmentResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<PagedResult<AppointmentResponseDto>>>> GetAll([FromQuery] PaginationParameters pagination, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetAllAsync(
                pagination,
                cancellationToken);

            return Ok(ApiResponseFactory.Success(result.Items, result.pagination, "Appointment", ResponseAction.RetrievedList));
        }


        /// <summary>
        /// Retrieves an appointment by identifier.
        /// </summary>
        /// <param name="id">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Appointment details.
        /// </returns>
        /// <response code="200">Appointment retrieved successfully.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Appointment not found.</response>

        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<AppointmentDetailsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<AppointmentDetailsResponseDto>>> GetById(int id, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentService.GetByIdAsync(id, cancellationToken);

            return Ok(ApiResponseFactory.Success(appointment, "Appointment", ResponseAction.Retrieved));
        }


        /// <summary>
        /// Creates a new appointment.
        /// </summary>
        /// <param name="request">
        /// Appointment information.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Newly created appointment.
        /// </returns>
        /// <response code="201">Appointment created successfully.</response>
        /// <response code="400">Invalid request.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Doctor or patient profile not found.</response>
        /// <response code="409">Appointment conflict.</response>

        [Authorize(Policy = "PatientOnly")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AppointmentResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<AppointmentResponseDto>>> Create(CreateAppointmentRequestDto request, CancellationToken cancellationToken)
        {
            var AddedAppointment = await _appointmentService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = AddedAppointment.Id }, ApiResponseFactory.Success(AddedAppointment, "Appointment", ResponseAction.Created));
        }

        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        /// <param name="id">
        /// Appointment identifier.
        /// </param>
        /// <param name="request">
        /// Updated appointment information.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Updated appointment.
        /// </returns>
        /// <response code="200">Appointment updated successfully.</response>
        /// <response code="400">Invalid request.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Appointment not found.</response>
        /// <response code="409">Appointment conflict.</response>

        [Authorize(Policy = "PatientOnly")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<AppointmentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<AppointmentResponseDto>>> Update(int id, UpdateAppointmentRequestDto request, CancellationToken cancellationToken)
        {
            var UpdatedAppointment = await _appointmentService.UpdateAsync(id, request, cancellationToken);

            return Ok(ApiResponseFactory.Success(UpdatedAppointment, "Appointment", ResponseAction.Updated));
        }

        /// <summary>
        /// Confirms a pending appointment.
        /// </summary>
        /// <param name="id">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The confirmed appointment.
        /// </returns>
        /// <response code="200">Appointment confirmed successfully.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Appointment not found.</response>
        /// <response code="409">Appointment cannot be confirmed.</response>
        [Authorize(Policy = "AdminOrDoctor")]
        [HttpPatch("{id:int}/confirm")]
        [ProducesResponseType(typeof(ApiResponse<AppointmentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<AppointmentResponseDto>>> Confirm(int id,CancellationToken cancellationToken)
        {
            var appointment = await _appointmentService.ConfirmAsync(id, cancellationToken);

            return Ok(ApiResponseFactory.Success(
                appointment,
                "Appointment",
                ResponseAction.Updated));
        }
        /// <summary>
        /// Marks a confirmed appointment as completed.
        /// </summary>
        /// <param name="id">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The completed appointment.
        /// </returns>
        /// <response code="200">Appointment completed successfully.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Appointment not found.</response>
        /// <response code="409">Appointment cannot be completed.</response>
        [Authorize(Policy = "DoctorOnly")]
        [HttpPatch("{id:int}/complete")]
        [ProducesResponseType(typeof(ApiResponse<AppointmentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<AppointmentResponseDto>>> Complete(int id,CancellationToken cancellationToken)
        {
            var appointment = await _appointmentService.CompleteAsync(id,cancellationToken);

            return Ok(ApiResponseFactory.Success(
                appointment,
                "Appointment",
                ResponseAction.Updated));
        }
        /// <summary>
        /// Cancels an appointment.
        /// </summary>
        /// <param name="id">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The cancelled appointment.
        /// </returns>
        /// <response code="200">Appointment cancelled successfully.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Appointment not found.</response>
        /// <response code="409">Appointment cannot be cancelled.</response>
        [Authorize(Policy = "AdminOrDoctor")]
        [HttpPatch("{id:int}/cancel")]
        [ProducesResponseType(typeof(ApiResponse<AppointmentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<AppointmentResponseDto>>> Cancel(int id,CancellationToken cancellationToken)
        {
            var appointment = await _appointmentService.CancelAsync(id,cancellationToken);

            return Ok(ApiResponseFactory.Success(
                appointment,
                "Appointment",
                ResponseAction.Updated));
        }

        /// <summary>
        /// Deletes an appointment.
        /// </summary>
        /// <param name="id">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Success message.
        /// </returns>
        /// <response code="200">Appointment deleted successfully.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Appointment not found.</response>
        /// <response code="409">Completed appointments cannot be deleted.</response>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _appointmentService.DeleteAsync(id, cancellationToken);

            return Ok(ApiResponseFactory.Success(true, "Appointment", ResponseAction.Deleted));
        }

    }
}
