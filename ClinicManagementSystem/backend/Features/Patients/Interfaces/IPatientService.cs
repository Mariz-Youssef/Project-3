using ClinicManagementSystem.backend.Features.Patients.DTOs;

namespace ClinicManagementSystem.backend.Features.Patients.Interfaces;

public interface IPatientService
{
    Task<PatientResponseDto> CreatePatientProfileAsync(int userId, CreatePatientDto dto, CancellationToken cancellationToken = default);

    Task<PatientResponseDto> GetPatientProfileAsync(int userId, CancellationToken cancellationToken = default);

    Task<PatientResponseDto> GetPatientByIdAsync(int patientId, CancellationToken cancellationToken = default);

    Task<PatientResponseDto> UpdatePatientProfileAsync(int userId, UpdatePatientDto dto, CancellationToken cancellationToken = default);

    Task<bool> DeletePatientAsync(int patientId, CancellationToken cancellationToken = default);

    Task<IEnumerable<PatientResponseDto>> SearchPatientsAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}