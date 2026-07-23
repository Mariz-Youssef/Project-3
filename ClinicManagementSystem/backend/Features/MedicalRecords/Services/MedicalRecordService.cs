/*using AutoMapper;
using ClinicManagementSystem.backend.Common.Exceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Enums;
using ClinicManagementSystem.backend.Features.Appointments.Interfaces;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs;
using ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Services;

/// <summary>
/// Implements medical record business operations and domain rule enforcement.
/// </summary>
public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _medicalRecordRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public MedicalRecordService(
        IMedicalRecordRepository medicalRecordRepository,
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IMapper mapper)
    {
        _medicalRecordRepository = medicalRecordRepository;
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task<MedicalRecordResponse> CreateAsync(int doctorUserId, CreateMedicalRecordRequest request, CancellationToken cancellationToken = default)
    {
        // Rule 3: Verify appointment exists and is Completed
        var appointment = await _appointmentRepository.GetByIdWithDetailsAsync(request.AppointmentId, cancellationToken)
            ?? throw new NotFoundException($"Appointment with ID {request.AppointmentId} was not found.");

        if (appointment.Status != AppointmentStatus.Completed)
        {
            throw new BadRequestException("Medical records can only be created for completed appointments.");
        }

        // Rule 1 & 4: Ensure the creating doctor is the doctor assigned to the appointment
        if (appointment.Doctor.UserId != doctorUserId)
        {
            throw new ForbiddenException("You are not authorized to create a medical record for this appointment.");
        }

        // Rule 2: Ensure strictly one medical record per appointment
        var existingRecord = await _medicalRecordRepository.GetByAppointmentIdAsync(request.AppointmentId, cancellationToken);
        if (existingRecord != null)
        {
            throw new ConflictException("A medical record already exists for this appointment.");
        }

        var medicalRecord = _mapper.Map<MedicalRecord>(request);
        medicalRecord.CreatedAt = DateTime.UtcNow;

        await _medicalRecordRepository.AddAsync(medicalRecord, cancellationToken);

        // Retrieve with full navigation details for complete DTO mapping
        var createdRecord = await _medicalRecordRepository.GetByIdWithDetailsAsync(medicalRecord.Id, cancellationToken)
            ?? throw new NotFoundException("Failed to retrieve the newly created medical record.");

        return _mapper.Map<MedicalRecordResponse>(createdRecord);
    }

    public async Task<MedicalRecordResponse> GetByIdAsync(int medicalRecordId, int currentUserId, bool isAdmin, bool isDoctor, bool isPatient, CancellationToken cancellationToken = default)
    {
        var record = await _medicalRecordRepository.GetByIdWithDetailsAsync(medicalRecordId, cancellationToken)
            ?? throw new NotFoundException($"Medical record with ID {medicalRecordId} was not found.");

        // Rule 5: RBAC access validation
        ValidateReadAccess(record, currentUserId, isAdmin, isDoctor, isPatient);

        return _mapper.Map<MedicalRecordResponse>(record);
    }

    public async Task<MedicalRecordResponse> UpdateAsync(int medicalRecordId, int doctorUserId, UpdateMedicalRecordRequest request, CancellationToken cancellationToken = default)
    {
        // Use tracked CQS query for mutation workflows
        var record = await _medicalRecordRepository.GetByIdForUpdateAsync(medicalRecordId, cancellationToken)
            ?? throw new NotFoundException($"Medical record with ID {medicalRecordId} was not found.");

        // Rule 4: Only the doctor assigned to the appointment can update the record
        if (record.Appointment.Doctor.UserId != doctorUserId)
        {
            throw new ForbiddenException("You are not authorized to update this medical record.");
        }

        _mapper.Map(request, record);
        record.UpdatedAt = DateTime.UtcNow;

        await _medicalRecordRepository.UpdateAsync(record, cancellationToken);

        // Fetch untracked detailed projection for response DTO
        var updatedRecord = await _medicalRecordRepository.GetByIdWithDetailsAsync(record.Id, cancellationToken)
            ?? throw new NotFoundException("Failed to retrieve the updated medical record.");

        return _mapper.Map<MedicalRecordResponse>(updatedRecord);
    }

    public async Task<PagedResult<MedicalHistoryResponse>> GetPatientHistoryAsync(int patientId, PaginationParameters pagination, int currentUserId, bool isAdmin, bool isDoctor, bool isPatient, CancellationToken cancellationToken = default)
    {
        if (pagination == null)
        {
            throw new BadRequestException("Pagination parameters cannot be null.");
        }

        var patient = await _patientRepository.GetByIdAsync(patientId, cancellationToken)
            ?? throw new NotFoundException($"Patient with ID {patientId} was not found.");

        // Rule 5: Patients can only view their own medical history
        if (isPatient && !isAdmin && !isDoctor)
        {
            if (patient.UserId != currentUserId)
            {
                throw new ForbiddenException("You are not authorized to view another patient's medical history.");
            }
        }

        var pagedRecords = await _medicalRecordRepository.GetPatientHistoryAsync(patientId, pagination, cancellationToken);
        var mappedItems = _mapper.Map<List<MedicalHistoryResponse>>(pagedRecords.Items);

        return new PagedResult<MedicalHistoryResponse>(mappedItems, pagedRecords.PaginationMetadata);
    }

    private static void ValidateReadAccess(MedicalRecord record, int currentUserId, bool isAdmin, bool isDoctor, bool isPatient)
    {
        if (isAdmin) return;

        if (isDoctor && record.Appointment.Doctor.UserId == currentUserId) return;

        if (isPatient && record.Appointment.Patient.UserId == currentUserId) return;

        throw new ForbiddenException("You do not have permission to view this medical record.");
    }
}*/