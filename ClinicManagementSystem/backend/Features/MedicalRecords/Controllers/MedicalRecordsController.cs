/*using System.Security.Claims;
using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Exceptions;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs;
using ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Controllers
{
    /// <summary>
    /// Provides endpoints for managing patient medical records and clinical histories.
    /// </summary>
    [ApiController]
    [Route("api/medical-records")]
    [Produces("application/json")]
    [Authorize]
    public sealed class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecordsController"/> class.
        /// </summary>
        /// <param name="medicalRecordService">
        /// Provides medical record business operations.
        /// </param>
        public MedicalRecordsController(
            IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        /// <summary>
        /// Creates a new medical record for a completed appointment.
        /// </summary>
        /// <param name="request">
        /// The medical record creation payload.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns the newly created medical record wrapped in a standardized API response.
        /// </returns>
        /// <response code="201">Medical record created successfully.</response>
        /// <response code="400">The request is invalid or the appointment is not completed.</response>
        /// <response code="401">Authentication is required.</response>
        /// <response code="403">The authenticated doctor is not assigned to this appointment.</response>
        /// <response code="404">The specified appointment was not found.</response>
        /// <response code="409">A medical record already exists for this appointment.</response>
        [Authorize(Roles = Roles.Doctor)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateMedicalRecordRequest request, CancellationToken cancellationToken)
        {
            var doctorUserId = GetCurrentUserId();
            var record = await _medicalRecordService.CreateAsync(doctorUserId, request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = record.Id }, ApiResponseFactory.Success(record, "MedicalRecord", ResponseAction.Created));
        }

        /// <summary>
        /// Retrieves a medical record by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the medical record.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns the requested medical record wrapped in a standardized API response.
        /// </returns>
        /// <response code="200">Medical record retrieved successfully.</response>
        /// <response code="400">The supplied identifier is invalid.</response>
        /// <response code="401">Authentication is required.</response>
        /// <response code="403">The authenticated user does not have permission to view this record.</response>
        /// <response code="404">Medical record not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var record = await _medicalRecordService.GetByIdAsync(
                id,
                GetCurrentUserId(),
                IsAdmin(),
                IsDoctor(),
                IsPatient(),
                cancellationToken);

            return Ok(ApiResponseFactory.Success(record, "MedicalRecord", ResponseAction.Retrieved));
        }

        /// <summary>
        /// Updates an existing medical record.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the medical record.
        /// </param>
        /// <param name="request">
        /// The updated medical record information.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns the updated medical record wrapped in a standardized API response.
        /// </returns>
        /// <response code="200">Medical record updated successfully.</response>
        /// <response code="400">The request payload is invalid.</response>
        /// <response code="401">Authentication is required.</response>
        /// <response code="403">The authenticated doctor is not authorized to modify this record.</response>
        /// <response code="404">Medical record not found.</response>
       // [Authorize(Roles = Roles.Doctor)]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicalRecordRequest request, CancellationToken cancellationToken)
        {
            var doctorUserId = GetCurrentUserId();
            var record = await _medicalRecordService.UpdateAsync(id, doctorUserId, request, cancellationToken);

            return Ok(ApiResponseFactory.Success(record, "MedicalRecord", ResponseAction.Updated));
        }

        /// <summary>
        /// Retrieves a paginated medical history for a specific patient.
        /// </summary>
        /// <param name="patientId">
        /// The unique identifier of the patient.
        /// </param>
        /// <param name="pagination">
        /// Pagination parameters that specify the page number and page size.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns a paginated collection of medical history records.
        /// </returns>
        /// <response code="200">Medical history retrieved successfully.</response>
        /// <response code="400">The supplied parameters are invalid.</response>
        /// <response code="401">Authentication is required.</response>
        /// <response code="403">The authenticated user is not authorized to view this patient's history.</response>
        /// <response code="404">Patient not found.</response>
        [HttpGet("/api/patients/{patientId:int}/medical-history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPatientHistory(int patientId, [FromQuery] PaginationParameters pagination, CancellationToken cancellationToken)
        {
            var history = await _medicalRecordService.GetPatientHistoryAsync(
                patientId,
                pagination,
                GetCurrentUserId(),
                IsAdmin(),
                IsDoctor(),
                IsPatient(),
                cancellationToken);

            return Ok(ApiResponseFactory.Success(history.Items, history.PaginationMetadata, "MedicalHistory", ResponseAction.RetrievedList));
        }

        #region Private Claim Helpers

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                           ?? User.FindFirstValue("sub")
                           ?? User.FindFirstValue("uid");

            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("User identity could not be verified from the authorization token.");
            }

            return userId;
        }

        private bool IsAdmin() => User.IsInRole(role.Admin);
        private bool IsDoctor() => User.IsInRole(Roles.Doctor);
        private bool IsPatient() => User.IsInRole(Roles.Patient);

        #endregion
    }
}*/