using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Leaves;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Doctors.Controllers;

[ApiController]
[Route("api/doctors/{doctorId:int}/leaves")]
[Authorize]
public class DoctorLeavesController : ControllerBase
{
    private readonly IDoctorLeaveService _leaveService;

    public DoctorLeavesController(
        IDoctorLeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    /// <summary>
    /// Retrieves all leaves for a doctor.
    /// </summary>
    [ProducesResponseType(typeof(ApiResponse<PagedResult<LeaveResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<IActionResult> GetByDoctor(int doctorId,[FromQuery] PaginationParameters pagination,CancellationToken cancellationToken)
    {
        var result = await _leaveService.GetByDoctorAsync(
            doctorId,
            pagination,
            cancellationToken);

        return Ok(ApiResponseFactory.Success(result,result.pagination,"Leave",ResponseAction.RetrievedList));
    }

    /// <summary>
    /// Retrieves a leave by its identifier.
    /// </summary>
    [ProducesResponseType(typeof(ApiResponse<LeaveResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int doctorId, int id,CancellationToken cancellationToken)
    {
        var result = await _leaveService.GetByIdAsync(doctorId,id, cancellationToken);

        return Ok(ApiResponseFactory.Success(result,"Leave",ResponseAction.Retrieved));
    }

    /// <summary>
    /// Creates a leave for a doctor.
    /// </summary>
    [ProducesResponseType(typeof(ApiResponse<LeaveResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [HttpPost]
    [Authorize(Policy = "AdminOrDoctor")]

    public async Task<IActionResult> Create(int doctorId,[FromBody] CreateLeaveRequest request,CancellationToken cancellationToken)
    {
        var result = await _leaveService.CreateAsync(doctorId,request,cancellationToken);

        return CreatedAtAction(nameof(GetById),
            new
            {
                doctorId,
                id = result.Id
            },
            ApiResponseFactory.Success(
                result,
                "Leave",
                ResponseAction.Created));

    }

    /// <summary>
    /// Updates a doctor's leave.
    /// </summary>
    [ProducesResponseType(typeof(ApiResponse<LeaveResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOrDoctor")]

    public async Task<IActionResult> Update(int doctorId,int id,UpdateLeaveRequest request,CancellationToken cancellationToken)
    {
        var result = await _leaveService.UpdateAsync(
            doctorId,
            id,
            request,
            cancellationToken);

        return Ok(ApiResponseFactory.Success(result,"Leave",ResponseAction.Updated));
    }

    /// <summary>
    /// Deletes a leave.
    /// </summary>
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOrDoctor")]

    public async Task<IActionResult> Delete(int doctorId,int id,CancellationToken cancellationToken)
    {
        await _leaveService.DeleteAsync(
            doctorId,
            id,
            cancellationToken);

        return Ok(ApiResponseFactory.Success("Leave",ResponseAction.Deleted));
    }
}
