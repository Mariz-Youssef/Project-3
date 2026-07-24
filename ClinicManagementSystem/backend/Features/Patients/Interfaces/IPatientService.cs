using ClinicManagementSystem.backend.Features.Patients.DTOs;
using ClinicManagementSystem.backend.Common.Pagination;

namespace ClinicManagementSystem.backend.Features.Patients.Interfaces;

public interface IPatientService
{
    Task<PatientResponseDto> CreatePatientProfileAsync(int userId, CreatePatientDto dto, CancellationToken cancellationToken = default);

    Task<PatientResponseDto> GetPatientProfileAsync(int userId, CancellationToken cancellationToken = default);

    Task<PatientResponseDto> GetPatientByIdAsync(int patientId, CancellationToken cancellationToken = default);

    Task<PatientResponseDto> UpdatePatientProfileAsync(int userId, UpdatePatientDto dto, CancellationToken cancellationToken = default);

    Task<bool> DeletePatientAsync(int patientId, CancellationToken cancellationToken = default);

    Task<PagedResult<PatientResponseDto>> GetAllPatientsAsync(PaginationParameters pagination, CancellationToken cancellationToken = default);

    Task<PagedResult<PatientResponseDto>> SearchPatientsAsync(string searchTerm, PaginationParameters pagination, CancellationToken cancellationToken = default);
}