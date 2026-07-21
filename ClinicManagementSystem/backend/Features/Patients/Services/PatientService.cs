using AutoMapper;
using ClinicManagementSystem.backend.Features.Patients.DTOs;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Patients.Services;

public sealed class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;
    private readonly IMapper _mapper;

    public PatientService(IPatientRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PatientResponseDto> CreatePatientProfileAsync(int userId, CreatePatientDto dto, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsAsync(patient => patient.UserId == userId, cancellationToken))
        {
            throw new InvalidOperationException("A patient profile already exists for this user.");
        }

        var patient = _mapper.Map<Patient>(dto);
        patient.UserId = userId;

        await _repository.AddAsync(patient, cancellationToken);

        var createdPatient = await _repository.GetPatientProfileWithUserAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException("Patient profile was not found after creation.");

        return _mapper.Map<PatientResponseDto>(createdPatient);
    }

    public async Task<PatientResponseDto> GetPatientProfileAsync(int userId, CancellationToken cancellationToken = default)
    {
        var patient = await _repository.GetPatientProfileWithUserAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException("Patient profile was not found.");

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto> GetPatientByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var patient = await _repository.GetPatientByIdWithUserAsync(patientId, cancellationToken)
            ?? throw new KeyNotFoundException("Patient profile was not found.");

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto> UpdatePatientProfileAsync(int userId, UpdatePatientDto dto, CancellationToken cancellationToken = default)
    {
        var patient = await _repository.GetPatientProfileWithUserAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException("Patient profile was not found.");

        _mapper.Map(dto, patient);

        await _repository.UpdateAsync(patient, cancellationToken);

        var updatedPatient = await _repository.GetPatientProfileWithUserAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException("Patient profile was not found after update.");

        return _mapper.Map<PatientResponseDto>(updatedPatient);
    }

    public async Task<bool> DeletePatientAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var patient = await _repository.GetByIdAsync(patientId, cancellationToken)
            ?? throw new KeyNotFoundException("Patient profile was not found.");

        await _repository.DeleteAsync(patient, cancellationToken);
        return true;
    }

    public async Task<IEnumerable<PatientResponseDto>> SearchPatientsAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var patients = await _repository.SearchPatientsWithUserAsync(searchTerm, pageNumber, pageSize, cancellationToken);

        return _mapper.Map<IEnumerable<PatientResponseDto>>(patients);
    }
}