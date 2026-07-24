using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Requests;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.DepartmentFeature.Controllers
{
    /// <summary>
    /// Provides endpoints for managing clinic departments.
    /// </summary>
    [ApiController]
    [Route("api/departments")]
    [Produces("application/json")]
    public sealed class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentsController"/> class.
        /// </summary>
        /// <param name="departmentService">
        /// Provides department business operations.
        /// </param>
        public DepartmentsController(
            IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Retrieves a paginated list of all departments.
        /// </summary>
        /// <param name="pagination">
        /// Pagination parameters that specify the page number and page size.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns a paginated collection of departments wrapped in a standardized API response.
        /// </returns>
        /// <response code="200">Departments retrieved successfully.</response>
        /// <response code="401">
        /// Authentication is required.
        /// </response>
        /// <response code="403">
        /// The authenticated user does not have permission to access this resource.
        /// </response>

        [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Doctor},{RoleNames.Patient}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParameters pagination, CancellationToken cancellationToken)
        {
            var result = await _departmentService.GetAllAsync(pagination, cancellationToken);

            return Ok(ApiResponseFactory.Success(result.Items, result.pagination, "Department", ResponseAction.RetrievedList));
        }

        /// <summary>
        /// Retrieves a department by its unique identifier.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the department.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns the requested department wrapped in a standardized API response.
        /// </returns>
        /// <response code="200">Department retrieved successfully.</response>
        /// <response code="400">The supplied department identifier is invalid.</response>
        /// <response code="401">Authentication is required.</response>
        /// <response code="403">The authenticated user is not authorized.</response>
        /// <response code="404">Department not found.</response>

      
        [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Doctor},{RoleNames.Patient}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var department = await _departmentService.GetByIdAsync(id, cancellationToken);

            return Ok(ApiResponseFactory.Success(department, "Department", ResponseAction.Retrieved));

        }

        /// <summary>
        /// Searches departments by name.
        /// </summary>
        /// <param name="searchTerm">
        /// The department name or part of the department name.
        /// </param>
        /// <param name="pagination">
        /// Pagination parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns a paginated collection of matching departments.
        /// </returns>
        /// <response code="200">Departments retrieved successfully.</response>
        /// <response code="400">The supplied search term is invalid.</response>
        /// <response code="401">Authentication is required.</response>
        /// <response code="403">The authenticated user is not authorized.</response>

        [Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Doctor},{RoleNames.Patient}")]
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Search([FromQuery] string searchTerm, [FromQuery] PaginationParameters pagination, CancellationToken cancellationToken)
        {
            var result = await _departmentService.SearchAsync(searchTerm, pagination, cancellationToken);

            return Ok(ApiResponseFactory.Success(result.Items, result.pagination, "Department", ResponseAction.RetrievedList));

        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="request">
        /// The department information.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns the newly created department.
        /// </returns>
        /// <response code="201">Department created successfully.</response>
        /// <response code="400">The request is invalid.</response>
        /// <response code="401">Authentication is required.</response>
        /// <response code="403">The authenticated user is not authorized.</response>
        /// <response code="409">Department already exists.</response>

       
        [Authorize(Roles = $"{RoleNames.Admin}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequestDto request, CancellationToken cancellationToken)
        {
            var department = await _departmentService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = department.Id }, ApiResponseFactory.Success(department, "Department", ResponseAction.Created));
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the department.
        /// </param>
        /// <param name="request">
        /// The updated department information.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns the updated department.
        /// </returns>
        /// <response code="200">Department updated successfully.</response>
        /// <response code="400">The request is invalid.</response>
        /// <response code="401">Authentication is required.</response>
        /// <response code="403">The authenticated user is not authorized.</response>
        /// <response code="404">Department not found.</response>
        /// <response code="409">Department already exists.</response>

        
        [Authorize(Roles = $"{RoleNames.Admin}")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartmentRequestDto request, CancellationToken cancellationToken)
        {
            var department = await _departmentService.UpdateAsync(id, request, cancellationToken);

            return Ok(ApiResponseFactory.Success(department, "Department", ResponseAction.Updated));

        }

        /// <summary>
        /// Soft deletes a department.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the department.
        /// </param>
        /// <param name="cancellationToken">
        /// A token used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// Returns a success response.
        /// </returns>
        /// <response code="200">Department deleted successfully.</response>
        /// <response code="400">The supplied identifier is invalid.</response>
        /// <response code="401">Authentication is required.</response>
        /// <response code="403">The authenticated user is not authorized.</response>
        /// <response code="404">Department not found.</response>

        [Authorize(Roles = $"{RoleNames.Admin}")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _departmentService.DeleteAsync(id, cancellationToken);

            return Ok(ApiResponseFactory.Success(true, "Department", ResponseAction.Deleted));

        }
    }
}