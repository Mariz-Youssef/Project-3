using ClinicManagementSystem.backend.Features.Patients.DTOs;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicManagementSystem.backend.Features.Patients.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost("profile")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> CreatePatientProfile([FromBody] CreatePatientDto dto, CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromClaims();
        var patient = await _patientService.CreatePatientProfileAsync(userId, dto, cancellationToken);

        return CreatedAtAction(nameof(GetPatientProfile), patient);
    }

    [HttpGet("profile")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetPatientProfile(CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromClaims();
        var patient = await _patientService.GetPatientProfileAsync(userId, cancellationToken);

        return Ok(patient);
    }

    [HttpPut("profile")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> UpdatePatientProfile([FromBody] UpdatePatientDto dto, CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromClaims();
        var patient = await _patientService.UpdatePatientProfileAsync(userId, dto, cancellationToken);

        return Ok(patient);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> GetPatientById([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var patient = await _patientService.GetPatientByIdAsync(id, cancellationToken);

        return Ok(patient);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePatient([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        await _patientService.DeletePatientAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> SearchPatients(
        [FromQuery] string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var patients = await _patientService.SearchPatientsAsync(searchTerm, pageNumber, pageSize, cancellationToken);

        return Ok(patients);
    }

    private int GetUserIdFromClaims()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}