using ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Doctors.Controllers
{
    /// <summary>
    /// Provides endpoints for managing doctors.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorsController"/> class.
        /// </summary>
        /// <param name="doctorService">Provides doctor-related business operations.</param>
        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        /// <summary>
        /// Retrieves all doctors.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of all doctors.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var doctors = await _doctorService.GetAllAsync(cancellationToken);
            return Ok(doctors);
        }
        /// <summary>
        /// Retrieves a doctor by identifier.
        /// </summary>
        /// <param name="id">The doctor's unique identifier.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The requested doctor.</returns>
        /// <response code="200">Doctor retrieved successfully.</response>
        /// <response code="404">Doctor not found.</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.GetByIdAsync(id, cancellationToken);
            return Ok(doctor);
        }
        /// <summary>
        /// Creates a new doctor.
        /// </summary>
        /// <param name="request">The doctor information.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The newly created doctor.</returns>
        /// <response code="201">Doctor created successfully.</response>
        /// <response code="400">The supplied data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorRequest request,CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.CreateAsync(request,cancellationToken);
            return CreatedAtAction(nameof(GetById),new { id = doctor.Id },doctor);
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
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,UpdateDoctorRequest request,CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.UpdateAsync(
                id,
                request,
                cancellationToken);

            return Ok(doctor);
        }
        /// <summary>
        /// Deletes a doctor.
        /// </summary>
        /// <param name="id">The doctor's unique identifier.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Doctor deleted successfully.</response>
        /// <response code="404">Doctor not found.</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            await _doctorService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        /// <summary>
        /// Retrieves all doctors belonging to a specific department.
        /// </summary>
        /// <param name="departmentId">The department identifier.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of doctors in the specified department.</returns>
        [HttpGet("department/{departmentId:int}")]
        public async Task<IActionResult> GetByDepartment(int departmentId,CancellationToken cancellationToken)
        {
            var doctors = await _doctorService.GetByDepartmentAsync(departmentId,cancellationToken);
            return Ok(doctors);
        }
        /// <summary>
        /// Retrieves all doctors with a specific specialization.
        /// </summary>
        /// <param name="specialization">The doctor's specialization.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of doctors with the specified specialization.</returns>
        [HttpGet("specialization/{specialization}")]
        public async Task<IActionResult> GetBySpecialization(string specialization,CancellationToken cancellationToken)
        {
            var doctors = await _doctorService.GetBySpecializationAsync(
                specialization,
                cancellationToken);

            return Ok(doctors);
        }
    }
}
