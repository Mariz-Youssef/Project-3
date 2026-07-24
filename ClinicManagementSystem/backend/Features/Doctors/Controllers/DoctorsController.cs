using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Responses;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Doctors.Controllers
{
    /// <summary>
    /// Provides endpoints for managing doctors.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        /// <summary>
        /// Retrieves all doctors.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of all doctors.</returns>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<DoctorResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters pagination,CancellationToken cancellationToken)
        {
            var doctors = await _doctorService.GetAllAsync(pagination,cancellationToken);
            return Ok(ApiResponse<PagedResult<DoctorResponse>>.SuccessResponse(doctors,"Doctors retrieved successfully."));
        }
        /// <summary>
        /// Retrieves a doctor by identifier.
        /// </summary>
        /// <param name="id">The doctor's unique identifier.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The requested doctor.</returns>
        /// <response code="200">Doctor retrieved successfully.</response>
        /// <response code="404">Doctor not found.</response>
        [ProducesResponseType(typeof(ApiResponse<DoctorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.GetByIdAsync(id, cancellationToken);
            return Ok(ApiResponse<DoctorResponse>.SuccessResponse(doctor,"Doctor retrieved successfully."));
        }
        /// <summary>
        /// Creates a new doctor.
        /// </summary>
        /// <param name="request">The doctor information.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The newly created doctor.</returns>
        /// <response code="201">Doctor created successfully.</response>
        /// <response code="400">The supplied data is invalid.</response>
        [ProducesResponseType(typeof(ApiResponse<DoctorResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(CreateDoctorRequest request,CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.CreateAsync(request,cancellationToken);
            return CreatedAtAction(nameof(GetById),new { id = doctor.Id },ApiResponse<DoctorResponse>.SuccessResponse(doctor,"Doctor created successfully."));
        }
        /// <summary>
        /// Updates an existing doctor.
        /// </summary>
        /// <param name="id">The doctor's unique identifier.</param>
        /// <param name="request">The updated doctor information.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The updated doctor.</returns>
        /// <response code="200">Doctor updated successfully.</response>
        /// <response code="404">Doctor not found.</response>
        [ProducesResponseType(typeof(ApiResponse<DoctorResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id,UpdateDoctorRequest request,CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.UpdateAsync(id,request,cancellationToken);
            return Ok(ApiResponse<DoctorResponse>.SuccessResponse(doctor,"Doctor updated successfully."));
        }
        /// <summary>
        /// Deletes a doctor.
        /// </summary>
        /// <param name="id">The doctor's unique identifier.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Doctor deleted successfully.</response>
        /// <response code="404">Doctor not found.</response>
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            await _doctorService.DeleteAsync(id, cancellationToken);
            return Ok(ApiResponse<object>.SuccessResponse(null,"Doctor deleted successfully."));
        }
        /// <summary>
        /// Retrieves all doctors belonging to a specific department.
        /// </summary>
        /// <param name="departmentId">The department identifier.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of doctors in the specified department.</returns>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<DoctorResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [HttpGet("department/{departmentId:int}")]
        public async Task<IActionResult> GetByDepartment(int departmentId,[FromQuery] PaginationParameters pagination,CancellationToken cancellationToken)
        {
            var doctors = await _doctorService.GetByDepartmentAsync(departmentId,pagination,cancellationToken);
            return Ok(ApiResponse<PagedResult<DoctorResponse>>.SuccessResponse(doctors,"Doctors retrieved successfully."));
        }
        /// <summary>
        /// Retrieves all doctors with a specific specialization.
        /// </summary>
        /// <param name="specialization">The doctor's specialization.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of doctors with the specified specialization.</returns>
        [ProducesResponseType(typeof(ApiResponse<PagedResult<DoctorResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [HttpGet("specialization/{specialization}")]
        public async Task<IActionResult> GetBySpecialization(string specialization,[FromQuery] PaginationParameters pagination,CancellationToken cancellationToken)
        {
            var doctors = await _doctorService.GetBySpecializationAsync(specialization,pagination,cancellationToken);
            return Ok(ApiResponse<PagedResult<DoctorResponse>>.SuccessResponse(doctors,"Doctors retrieved successfully."));
        }
    }
}
