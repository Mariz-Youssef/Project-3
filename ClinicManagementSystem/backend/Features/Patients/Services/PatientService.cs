using AutoMapper;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Patients.DTOs;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.backend.Features.Patients.Services;

public sealed class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    private readonly UserManager<ApplicationUser> _userManager;

    public PatientService(IPatientRepository repository, IApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<PatientResponseDto> CreatePatientProfileAsync(int userId, CreatePatientDto dto, CancellationToken cancellationToken = default)
    {
        //Check if dto is equal null or not
        ArgumentNullException.ThrowIfNull(dto);

        // Validate user id
          ValidateUserId(userId);

           // Ensure the ApplicationUser exists
           ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            throw new NotFoundException($"User with ID '{userId}' was not found.");
        }

        // Ensure the user belongs to the Patient role
        bool isPatient = await _userManager.IsInRoleAsync(user, "Patient");

        if (!isPatient)
        {
            throw new ConflictException(
                "Only users assigned to the Patient role can create a patient profile.");
        }

        // Prevent duplicate patient profile
        bool profileExists = await _repository.ExistsAsync(
            patient => patient.UserId == userId,
            cancellationToken);

        if (profileExists)
        {
            throw new ConflictException(
                "A patient profile already exists for this user.");
        }

        // Map DTO -> Entity
        Patient patient = _mapper.Map<Patient>(dto);

        patient.UserId = userId;

        await _repository.AddAsync(patient, cancellationToken);

        int rowsAffected = await _context.SaveChangesAsync(cancellationToken);

        if (rowsAffected == 0)
        {
            throw new InvalidOperationException(
                "Failed to create the patient profile.");
        }

        Patient createdPatient =
            await _repository.GetPatientProfileWithUserAsync(
                userId,
                cancellationToken)
            ?? throw new NotFoundException(
                "Patient profile was not found after creation.");

        return _mapper.Map<PatientResponseDto>(createdPatient);



    }

    public async Task<PatientResponseDto> GetPatientProfileAsync(int userId, CancellationToken cancellationToken = default)
    {
        var patient = await _repository.GetPatientProfileWithUserAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Patient profile was not found.");

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto> GetPatientByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var patient = await _repository.GetPatientByIdWithUserAsync(patientId, cancellationToken)
            ?? throw new NotFoundException("Patient profile was not found.");

        return _mapper.Map<PatientResponseDto>(patient);
    }

    public async Task<PatientResponseDto> UpdatePatientProfileAsync(int userId, UpdatePatientDto dto, CancellationToken cancellationToken = default)
    {
        var patient = await _repository.GetPatientProfileWithUserAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Patient profile was not found.");

        _mapper.Map(dto, patient);

        _repository.Update(patient);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedPatient = await _repository.GetPatientProfileWithUserAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Patient profile was not found after update.");

        return _mapper.Map<PatientResponseDto>(updatedPatient);
    }

    public async Task<bool> DeletePatientAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var patient = await _repository.GetByIdAsync(patientId, cancellationToken)
            ?? throw new NotFoundException("Patient profile was not found.");

        _repository.Delete(patient);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<PagedResult<PatientResponseDto>> GetAllPatientsAsync(PaginationParameters pagination, CancellationToken cancellationToken = default)
    {
        if (pagination is null)
        {
            throw new BadRequestException("Pagination parameters are required.");
        }

        PagedResult<Patient> pagedPatients = await _repository.GetAllPatientsPagedAsync(pagination, cancellationToken);

        return new PagedResult<PatientResponseDto>
        {
            Items = _mapper.Map<IReadOnlyList<PatientResponseDto>>(pagedPatients.Items),
            pagination = pagedPatients.pagination
        };
    }

    public async Task<PagedResult<PatientResponseDto>> SearchPatientsAsync(string searchTerm, PaginationParameters pagination, CancellationToken cancellationToken = default)
    {
        if (pagination is null)
        {
            throw new BadRequestException("Pagination parameters are required.");
        }

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            throw new BadRequestException("Search term cannot be empty.");
        }

        string normalizedSearch = searchTerm.Trim();

        PagedResult<Patient> pagedPatients = await _repository.SearchPatientsPagedAsync(normalizedSearch, pagination, cancellationToken);

        return new PagedResult<PatientResponseDto>
        {
            Items = _mapper.Map<IReadOnlyList<PatientResponseDto>>(pagedPatients.Items),
            pagination = pagedPatients.pagination
        };
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
        {
            throw new BadRequestException("User ID must be greater than zero.");
        }
    }
}