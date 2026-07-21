using ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Doctors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var doctors = await _doctorService.GetAllAsync(cancellationToken);
            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.GetByIdAsync(id, cancellationToken);
            return Ok(doctor);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDoctorRequest request,CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.CreateAsync(request,cancellationToken);
            return CreatedAtAction(nameof(GetById),new { id = doctor.Id },doctor);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,UpdateDoctorRequest request,CancellationToken cancellationToken)
        {
            var doctor = await _doctorService.UpdateAsync(
                id,
                request,
                cancellationToken);

            return Ok(doctor);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            await _doctorService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        [HttpGet("department/{departmentId:int}")]
        public async Task<IActionResult> GetByDepartment(int departmentId,CancellationToken cancellationToken)
        {
            var doctors = await _doctorService.GetByDepartmentAsync(departmentId,cancellationToken);
            return Ok(doctors);
        }
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
