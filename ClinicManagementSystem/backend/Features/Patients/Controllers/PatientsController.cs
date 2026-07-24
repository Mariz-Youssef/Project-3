using System.Security.Claims;
using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.Patients.DTOs;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Patients.Controllers;

/// <summary>
/// Provides endpoints for managing patient profiles and patient records.
/// </summary>
[ApiController]
[Route("api/patients")]
[Produces("application/json")]
public sealed class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientsController"/> class.
    /// </summary>
    /// <param name="patientService">
    /// Provides patient business operations.
    /// </param>
    
    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Retrieves a paginated list of all patient profiles.
    /// </summary>
    /// <param name="pagination">
    /// Pagination parameters that specify the page number and page size.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// Returns a paginated collection of patients wrapped in a standardized API response.
    /// </returns>
    
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Doctor}")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllPatients([FromQuery] PaginationParameters pagination, CancellationToken cancellationToken = default)
    {
        PagedResult<PatientResponseDto> result = await _patientService.GetAllPatientsAsync(pagination, cancellationToken);

        return Ok(ApiResponseFactory.Success(result.Items, result.pagination, "Patient", ResponseAction.RetrievedList));
    }

    /// <summary>
    /// Creates a patient profile for the authenticated patient user.
    /// </summary>
    /// <param name="request">
    /// The patient profile information.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// Returns the newly created patient profile wrapped in a standardized API response.
    /// </returns>
    
    [Authorize(Roles = RoleNames.Patient)]
    [HttpPost("profile")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreatePatientProfile([FromBody] CreatePatientDto request, CancellationToken cancellationToken = default)
    {
        int userId = GetUserIdFromClaims();
        PatientResponseDto patient = await _patientService.CreatePatientProfileAsync(userId, request, cancellationToken);

        return CreatedAtAction(
            nameof(GetPatientById),
            new { id = patient.Id },
            ApiResponseFactory.Success(patient, "Patient", ResponseAction.Created));
    }

    /// <summary>
    /// Retrieves the authenticated patient's profile.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// Returns the patient profile wrapped in a standardized API response.
    /// </returns>
    
    [Authorize(Roles = RoleNames.Patient)]
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientProfile(CancellationToken cancellationToken = default)
    {
        int userId = GetUserIdFromClaims();
        PatientResponseDto patient = await _patientService.GetPatientProfileAsync(userId, cancellationToken);

        return Ok(ApiResponseFactory.Success(patient, "Patient", ResponseAction.Retrieved));
    }

    /// <summary>
    /// Updates the authenticated patient's profile.
    /// </summary>
    /// <param name="request">
    /// The updated patient profile information.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// Returns the updated patient profile wrapped in a standardized API response.
    /// </returns>
    
    [Authorize(Roles = RoleNames.Patient)]
    [HttpPut("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePatientProfile([FromBody] UpdatePatientDto request, CancellationToken cancellationToken = default)
    {
        int userId = GetUserIdFromClaims();
        PatientResponseDto patient = await _patientService.UpdatePatientProfileAsync(userId, request, cancellationToken);

        return Ok(ApiResponseFactory.Success(patient, "Patient", ResponseAction.Updated));
    }

    /// <summary>
    /// Retrieves a patient profile by its unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the patient profile.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// Returns the patient profile wrapped in a standardized API response.
    /// </returns>
    
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Doctor}")]
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientById([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        PatientResponseDto patient = await _patientService.GetPatientByIdAsync(id, cancellationToken);

        return Ok(ApiResponseFactory.Success(patient, "Patient", ResponseAction.Retrieved));
    }

    /// <summary>
    /// Deletes a patient profile by its unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the patient profile.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// Returns a standardized success response.
    /// </returns>
   
    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePatient([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        bool isDeleted = await _patientService.DeletePatientAsync(id, cancellationToken);

        return Ok(ApiResponseFactory.Success(isDeleted, "Patient", ResponseAction.Deleted));
    }

    /// <summary>
    /// Searches patients using the supplied search term.
    /// </summary>
    /// <param name="searchTerm">
    /// The search text.
    /// </param>
    /// <param name="pagination">
    /// Pagination parameters.
    /// </param>
    /// <param name="cancellationToken">
    /// A token used to cancel the asynchronous operation.
    /// </param>
    /// <returns>
    /// Returns the matching patient profiles wrapped in a standardized API response.
    /// </returns>
    
    [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Doctor}")]
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SearchPatients(
        [FromQuery] string searchTerm,
        [FromQuery] PaginationParameters pagination,
        CancellationToken cancellationToken = default)
    {
        PagedResult<PatientResponseDto> result = await _patientService.SearchPatientsAsync(searchTerm, pagination, cancellationToken);

        return Ok(ApiResponseFactory.Success(result.Items, result.pagination, "Patient", ResponseAction.RetrievedList));
    }

    private int GetUserIdFromClaims()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}