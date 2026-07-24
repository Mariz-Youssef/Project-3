using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.WorkingHours;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Doctors.Controllers
{
    /// <summary>
    /// Provides endpoints for managing doctors' working hours.
    /// </summary>
    [ApiController]
    [Route("api/doctors/{doctorId:int}/working-hours")]
    [Authorize]
    public class DoctorWorkingHoursController : ControllerBase
    {
        private readonly IDoctorWorkingHourService _workingHourService;
        public DoctorWorkingHoursController(IDoctorWorkingHourService workingHourService)
        {
            _workingHourService = workingHourService;
        }

        /// <summary>
        /// Retrieves all working hours for a specific doctor.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="pagination">Pagination parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A paginated list of working hours.</returns>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<WorkingHourResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetByDoctor(int doctorId,[FromQuery] PaginationParameters pagination,CancellationToken cancellationToken)
        {
            var result = await _workingHourService.GetByDoctorAsync(doctorId,pagination,cancellationToken);

            return Ok(ApiResponse<PagedResult<WorkingHourResponse>>.SuccessResponse(result));
        }

        /// <summary>
        /// Retrieves a working-hour record by its identifier.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="id">The working-hour identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The requested working-hour record.</returns>
        [ProducesResponseType(typeof(ApiResponse<WorkingHourResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int doctorId,int id,CancellationToken cancellationToken)
        {
            var result = await _workingHourService.GetByIdAsync(id,cancellationToken);

            return Ok(ApiResponse<WorkingHourResponse>.SuccessResponse(result));
        }

        /// <summary>
        /// Creates a working-hour record for the specified doctor.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="request">The working-hour information.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The newly created working-hour record.</returns>
        [ProducesResponseType(typeof(ApiResponse<WorkingHourResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [HttpPost]
        [Authorize(Policy="AdminOrDoctor")]
        public async Task<IActionResult> Create(int doctorId,[FromBody] CreateWorkingHourRequest request,CancellationToken cancellationToken)
        {
            var result = await _workingHourService.CreateAsync(doctorId,request,cancellationToken);

            return CreatedAtAction(nameof(GetById),
                new
                {
                    doctorId,
                    id = result.Id
                },
                ApiResponse<WorkingHourResponse>.SuccessResponse(result));
        }

        /// <summary>
        /// Updates a working-hour record.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="id">The working-hour identifier.</param>
        /// <param name="request">The updated working-hour information.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated working-hour record.</returns>
        [ProducesResponseType(typeof(ApiResponse<WorkingHourResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOrDoctor")]
        public async Task<IActionResult> Update(int doctorId,int id,[FromBody] UpdateWorkingHourRequest request,CancellationToken cancellationToken)
        {
            var result = await _workingHourService.UpdateAsync(
                doctorId,
                id,
                request,
                cancellationToken);

            return Ok(ApiResponse<WorkingHourResponse>.SuccessResponse(result));
        }

        /// <summary>
        /// Deletes a working-hour record.
        /// </summary>
        /// <param name="doctorId">The doctor identifier.</param>
        /// <param name="id">The working-hour identifier.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>No content.</returns>
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOrDoctor")]
        public async Task<IActionResult> Delete(int doctorId,int id,CancellationToken cancellationToken)
        {
            await _workingHourService.DeleteAsync(
                doctorId,
                id,
                cancellationToken);

            return Ok(ApiResponse<object>.SuccessResponse(null,"Working hour deleted successfully."));
        }
    }
}