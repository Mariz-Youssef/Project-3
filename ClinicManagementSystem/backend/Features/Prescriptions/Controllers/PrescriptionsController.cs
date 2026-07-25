using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Responses;
using ClinicManagementSystem.backend.Features.Prescriptions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Prescriptions.Controllers
{
    /// <summary>
    /// Provides endpoints for managing prescriptions.
    /// </summary>
    [Route("api/Prescriptions")]
    [ApiController]
    [Authorize]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionsService _prescriptionsService;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PrescriptionsController"/> class.
        /// </summary>
        public PrescriptionsController(
            IPrescriptionsService prescriptionsService)
        {
            _prescriptionsService = prescriptionsService;
        }

        /// <summary>
        /// Retrieves paginated prescriptions.
        /// </summary>
        /// <remarks>
        /// Authorization:
        ///
        /// Admin:
        ///     Returns all prescriptions.
        ///
        /// Doctor:
        ///     Returns only prescriptions belonging to
        ///     the authenticated doctor's patients.
        ///
        /// Patient:
        ///     Returns only prescriptions that belong
        ///     to the authenticated patient.
        /// </remarks>
        /// <param name="pagination">
        /// Pagination parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Paginated prescriptions.
        /// </returns>
        /// <response code="200">
        /// Prescriptions retrieved successfully.
        /// </response>
        /// <response code="401">
        /// User is not authenticated.
        /// </response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<PrescriptionResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters pagination, CancellationToken cancellationToken)
        {
            var prescriptions = await _prescriptionsService.GetAllAsync(pagination, cancellationToken);

            return Ok(ApiResponseFactory.Success(prescriptions, "Prescriptions", ResponseAction.RetrievedList));

        }

        /// <summary>
        /// Retrieves a prescription by its identifier.
        /// </summary>
        /// <param name="id">
        /// Prescription identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Prescription details.
        /// </returns>
        /// <response code="200">
        /// Prescription retrieved successfully.
        /// </response>
        /// <response code="401">
        /// User is not authenticated.
        /// </response>
        /// <response code="404">
        /// Prescription was not found.
        /// </response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<PrescriptionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var prescription = await _prescriptionsService.GetByIdAsync(id, cancellationToken);

            return Ok(ApiResponseFactory.Success(prescription, "Prescriptions", ResponseAction.Retrieved));

        }

        /// <summary>
        /// Creates a new prescription.
        /// </summary>
        /// <remarks>
        /// Only doctors are allowed to create prescriptions.
        /// </remarks>
        /// <param name="request">
        /// Prescription information.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Newly created prescription.
        /// </returns>
        /// <response code="201">
        /// Prescription created successfully.
        /// </response>
        /// <response code="400">
        /// Validation failed.
        /// </response>
        /// <response code="401">
        /// User is not authenticated.
        /// </response>
        /// <response code="403">
        /// Only doctors are allowed.
        /// </response>
        /// <response code="404">
        /// Medical record was not found.
        /// </response>
        [HttpPost]
        [Authorize(Policy = "DoctorOnly")]
        [ProducesResponseType(typeof(ApiResponse<PrescriptionResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionRequestDto request, CancellationToken cancellationToken)
        {
            var prescription = await _prescriptionsService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = prescription.Id }, ApiResponseFactory.Success(prescription, "Prescriptions", ResponseAction.Created));

        }


        /// <summary>
        /// Updates an existing prescription.
        /// </summary>
        /// <remarks>
        /// Only doctors are allowed to update prescriptions.
        /// </remarks>
        /// <param name="id">
        /// Prescription identifier.
        /// </param>
        /// <param name="request">
        /// Updated prescription information.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Updated prescription.
        /// </returns>
        /// <response code="200">
        /// Prescription updated successfully.
        /// </response>
        /// <response code="400">
        /// Validation failed.
        /// </response>
        /// <response code="401">
        /// User is not authenticated.
        /// </response>
        /// <response code="403">
        /// Only doctors are allowed.
        /// </response>
        /// <response code="404">
        /// Prescription was not found.
        /// </response>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "DoctorOnly")]
        [ProducesResponseType(typeof(ApiResponse<PrescriptionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePrescriptionRequestDto request, CancellationToken cancellationToken)
        {
            var prescription = await _prescriptionsService.UpdateAsync(id, request, cancellationToken);

            return Ok(ApiResponseFactory.Success(prescription, "Prescriptions", ResponseAction.Updated));

        }

        /// <summary>
        /// Deletes a prescription.
        /// </summary>
        /// <remarks>
        /// Only administrators are allowed to delete prescriptions.
        /// </remarks>
        /// <param name="id">
        /// Prescription identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// True when the prescription is deleted successfully.
        /// </returns>
        /// <response code="200">
        /// Prescription deleted successfully.
        /// </response>
        /// <response code="401">
        /// User is not authenticated.
        /// </response>
        /// <response code="403">
        /// User is not authorized.
        /// </response>
        /// <response code="404">
        /// Prescription was not found.
        /// </response>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _prescriptionsService.DeleteAsync(id, cancellationToken);

            return Ok(ApiResponseFactory.Success(true, "Prescriptions", ResponseAction.Deleted));

        }

    }
}
