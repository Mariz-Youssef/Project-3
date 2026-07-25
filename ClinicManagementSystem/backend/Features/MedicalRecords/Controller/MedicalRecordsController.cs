using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Requests;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Responses;
using ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;
using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Controller
{
    /// <summary>
    /// Provides endpoints for managing medical records.
    /// </summary>
    [Route("api/MedicalRecords")]
    [ApiController]
    [Authorize]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordsService _medicalRecordService;

        public MedicalRecordsController(IMedicalRecordsService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;

        }

        /// <summary>
        /// Retrieves paginated medical records.
        /// </summary>
        /// <remarks>
        /// Authorization:
        ///
        /// Admin:
        ///     Returns all medical records.
        ///
        /// Doctor:
        ///     Returns only medical records created by the authenticated doctor.
        ///
        /// Patient:
        ///     Returns only the authenticated patient's medical records.
        /// </remarks>
        /// <param name="pagination">
        /// Pagination parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Paginated medical records.
        /// </returns>
        /// <response code="200">Medical records retrieved successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MedicalRecordResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters pagination, CancellationToken cancellationToken)

        {
            var records = await _medicalRecordService.GetAllAsync(
                pagination,
                cancellationToken);

            return Ok(ApiResponseFactory.Success(records, "MedicalRecords", ResponseAction.RetrievedList));

        }

        /// <summary>
        /// Retrieves a medical record by its identifier.
        /// </summary>
        /// <param name="id">
        /// Medical record identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Medical record details.
        /// </returns>
        /// <response code="200">Medical record retrieved successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="404">Medical record was not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<MedicalRecordResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var medicalRecord = await _medicalRecordService.GetByIdAsync(id, cancellationToken);
            return Ok(ApiResponseFactory.Success(medicalRecord, "MedicalRecords", ResponseAction.Retrieved));
        }

        /// <summary>
        /// Creates a new medical record.
        /// </summary>
        /// <remarks>
        /// Only doctors are allowed to create medical records.
        /// </remarks>
        /// <param name="request">
        /// Medical record information.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// Newly created medical record.
        /// </returns>
        /// <response code="201">Medical record created successfully.</response>
        /// <response code="400">Validation failed.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="403">Only doctors are allowed.</response>
        /// <response code="404">Appointment was not found.</response>
        /// <response code="409">Medical record already exists.</response>
        [HttpPost]
        [Authorize(Policy = "DoctorOnly")]
        [ProducesResponseType(typeof(ApiResponse<MedicalRecordResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateMedicalRecordRequestDto request, CancellationToken cancellationToken)
        {
            var medicalRecord = await _medicalRecordService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = medicalRecord.Id }, ApiResponseFactory.Success(medicalRecord, "MedicalRecords", ResponseAction.Created));
        }

        /// <summary>
        /// Updates an existing medical record.
        /// </summary>
        /// <remarks>
        /// Only doctors are allowed to update medical records.
        ///
        /// The authenticated doctor can update only medical records
        /// that belong to appointments assigned to him.
        /// </remarks>
        /// <param name="id">
        /// Medical record identifier.
        /// </param>
        /// <param name="request">
        /// Updated medical record information.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The updated medical record.
        /// </returns>
        /// <response code="200">
        /// Medical record updated successfully.
        /// </response>
        /// <response code="400">
        /// The request contains invalid data.
        /// </response>
        /// <response code="401">
        /// User is not authenticated.
        /// </response>
        /// <response code="403">
        /// The authenticated doctor is not authorized to update this medical record.
        /// </response>
        /// <response code="404">
        /// Medical record or related appointment was not found.
        /// </response>
        /// <response code="409">
        /// The requested operation violates business rules.
        /// </response>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "DoctorOnly")]
        [ProducesResponseType(typeof(ApiResponse<MedicalRecordResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalRecordRequestDto request, CancellationToken cancellationToken)
        {
            var medicalRecord = await _medicalRecordService.UpdateAsync(id, request, cancellationToken);

            return Ok(ApiResponseFactory.Success(medicalRecord, "MedicalRecords", ResponseAction.Updated));
        }

        /// <summary>
        /// Deletes a medical record.
        /// </summary>
        /// <remarks>
        /// Only administrators are allowed to delete medical records.
        ///
        /// The medical record is removed using the application's
        /// soft-delete mechanism.
        /// </remarks>
        /// <param name="id">
        /// Medical record identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// A successful deletion response.
        /// </returns>
        /// <response code="200">
        /// Medical record deleted successfully.
        /// </response>
        /// <response code="401">
        /// User is not authenticated.
        /// </response>
        /// <response code="403">
        /// Only administrators are authorized to delete medical records.
        /// </response>
        /// <response code="404">
        /// Medical record was not found.
        /// </response>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _medicalRecordService.DeleteAsync(id, cancellationToken);

            return Ok(ApiResponseFactory.Success(true, "MedicalRecords", ResponseAction.Deleted));
        }

        
    }
}
