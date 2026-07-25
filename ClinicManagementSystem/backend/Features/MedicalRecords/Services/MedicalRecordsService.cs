using AutoMapper;
using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Services.Interfaces;
using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Enums;
using ClinicManagementSystem.backend.Features.Appointments.Interfaces;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Requests;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Responses;
using ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Services
{
    public class MedicalRecordsService : IMedicalRecordsService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ILogger<MedicalRecordsService> _logger;

        //Constructor
        public MedicalRecordsService(
            IMedicalRecordRepository medicalRecordRepository,
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IApplicationDbContext context,
            ILogger<MedicalRecordsService> logger)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponseDto> CreateAsync(CreateMedicalRecordRequestDto request, CancellationToken cancellationToken = default)
        {
            // Ensure the request object is not null.
            ArgumentNullException.ThrowIfNull(request);

            // ---------------------------------------------------------------------
            // STEP 1:
            // Log the beginning of the create operation.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "Doctor User {UserId} started creating a medical record for appointment {AppointmentId}.",
                _currentUserService.UserId,
                request.AppointmentId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Validate the follow-up date.
            // ---------------------------------------------------------------------
            ValidateFollowUpDate(request.FollowUpDate);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Retrieve the appointment to check if exist or not.
            // ---------------------------------------------------------------------
            Appointment appointment = await ValidateAppointmentExistsAsync(request.AppointmentId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 4:
            // Ensure the appointment has already been completed.
            // ---------------------------------------------------------------------
            ValidateAppointmentCompleted(appointment);

            // ---------------------------------------------------------------------
            // STEP 5:
            // Ensure the authenticated doctor owns this appointment.
            // ---------------------------------------------------------------------
            await ValidateDoctorOwnsAppointmentAsync(appointment,cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 6:
            // Ensure no medical record already exists.
            // ---------------------------------------------------------------------
            await ValidateMedicalRecordNotExistsAsync(appointment.Id, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 7:
            // Convert DTO into entity.
            // ---------------------------------------------------------------------
            MedicalRecord medicalRecord = _mapper.Map<MedicalRecord>(request);

            // ---------------------------------------------------------------------
            // STEP 8:
            // Add the medical record.
            // ---------------------------------------------------------------------
            await _medicalRecordRepository.AddAsync(medicalRecord, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 9:
            // Persist changes.
            // ---------------------------------------------------------------------
            await _context.SaveChangesAsync(cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 10:
            // Reload the entity with all navigation properties.
            // ---------------------------------------------------------------------
            medicalRecord = await _medicalRecordRepository.
                GetByIdWithDetailsAsync(medicalRecord.Id, cancellationToken) ?? throw new InvalidOperationException("Failed to reload the created medical record.");

            // ---------------------------------------------------------------------
            // STEP 11:
            // Log successful creation.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "Medical record {MedicalRecordId} created successfully for appointment {AppointmentId}.",
                medicalRecord!.Id,
                medicalRecord.AppointmentId);

            // ---------------------------------------------------------------------
            // STEP 12:
            // Return the response DTO.
            // ---------------------------------------------------------------------
            return _mapper.Map<MedicalRecordResponseDto>(medicalRecord);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int medicalRecordId, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the medical record identifier.
            // ---------------------------------------------------------------------
            ValidateMedicalRecordId(medicalRecordId);


            // ---------------------------------------------------------------------
            // STEP 2:
            // Retrieve the medical record.
            // ---------------------------------------------------------------------
            MedicalRecord medicalRecord = await GetMedicalRecordOrThrowAsync(medicalRecordId, cancellationToken);

      
            // ---------------------------------------------------------------------
            // STEP 3:
            // Delete the medical record.
            // ---------------------------------------------------------------------
            _medicalRecordRepository.Delete(medicalRecord);

            // ---------------------------------------------------------------------
            // STEP 4:
            // Persist changes.
            // ---------------------------------------------------------------------
            await _context.SaveChangesAsync(cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 5:
            // Log successful deletion.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "Medical record {MedicalRecordId} deleted successfully.",
                medicalRecord.Id);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(int medicalRecordId, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the medical record identifier.
            // ---------------------------------------------------------------------
            ValidateMedicalRecordId(medicalRecordId);

            //Return if this medical record exists or not
            return await _medicalRecordRepository.ExistsAsync(medicalRecordId, cancellationToken);

        }

        /// <inheritdoc/>
        public async Task<PagedResult<MedicalRecordResponseDto>> GetAllAsync(PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Log the request.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "User {UserId} requested medical records.",
                _currentUserService.UserId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Start with the query including all required navigation properties.
            // ---------------------------------------------------------------------
            IQueryable<MedicalRecord> query = _medicalRecordRepository
                .QueryWithDetails()
                .AsNoTracking();

            // ---------------------------------------------------------------------
            // STEP 3:
            // Apply role-based filtering.
            // ---------------------------------------------------------------------

            if (_currentUserService.IsInRole(RoleNames.Admin))
            {
                // Admin can view all records.
            }
            else if (_currentUserService.IsInRole(RoleNames.Doctor))
            {
                query = query.Where(record =>
                    record.Appointment.Doctor.UserId ==
                    _currentUserService.UserId);
            }
            else if (_currentUserService.IsInRole(RoleNames.Patient))
            {
                query = query.Where(record =>
                    record.Appointment.Patient.UserId ==
                    _currentUserService.UserId);
            }
            else
            {
                throw new ForbiddenException(
                    "You are not authorized to access medical records.");
            }

            // ---------------------------------------------------------------------
            // STEP 4:
            // Order the records.
            // ---------------------------------------------------------------------
            query = query.OrderByDescending(record => record.CreatedAt);

            // ---------------------------------------------------------------------
            // STEP 5:
            // Apply pagination.
            // ---------------------------------------------------------------------
            PagedResult<MedicalRecord> pagedMedicalRecords =
                await query.ToPagedResultAsync(
                    pagination,
                    cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 6:
            // Map to DTOs.
            // ---------------------------------------------------------------------
            IReadOnlyList<MedicalRecordResponseDto> records =
                _mapper.Map<IReadOnlyList<MedicalRecordResponseDto>>(
                    pagedMedicalRecords.Items);

            // ---------------------------------------------------------------------
            // STEP 7:
            // Return the paginated response.
            // ---------------------------------------------------------------------
            return new PagedResult<MedicalRecordResponseDto>
            {
                Items = records,
                pagination = pagedMedicalRecords.pagination
            };
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponseDto> GetByIdAsync(int medicalRecordId, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the medical record identifier.
            // ---------------------------------------------------------------------
            ValidateMedicalRecordId(medicalRecordId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Log the request.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "User {UserId} requested medical record {MedicalRecordId}.",
                _currentUserService.UserId,
                medicalRecordId);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Retrieve the medical record with all related entities.
            // ---------------------------------------------------------------------
            MedicalRecord? medicalRecord = await _medicalRecordRepository.GetByIdWithDetailsAsync(medicalRecordId, cancellationToken);

            if (medicalRecord is null)
            {
                throw new NotFoundException($"Medical record with ID '{medicalRecordId}' was not found.");

            }

            // ---------------------------------------------------------------------
            // STEP 4:
            // Apply authorization based on the authenticated user's role.
            // ---------------------------------------------------------------------

            if (_currentUserService.IsInRole(RoleNames.Admin))
            {
                // Admin can access any medical record.
            }
            else if (_currentUserService.IsInRole(RoleNames.Doctor))
            {
                if (medicalRecord.Appointment.Doctor.UserId != _currentUserService.UserId)
                {
                    throw new ForbiddenException("You are not allowed to access this medical record.");

                }
            }
            else if (_currentUserService.IsInRole(RoleNames.Patient))
            {
                if (medicalRecord.Appointment.Patient.UserId != _currentUserService.UserId)
                {
                    throw new ForbiddenException("You are not allowed to access this medical record.");

                }
            }
            else
            {
                throw new ForbiddenException("You are not authorized to access medical records.");

            }

            // ---------------------------------------------------------------------
            // STEP 5:
            // Return the response DTO.
            // ---------------------------------------------------------------------
            return _mapper.Map<MedicalRecordResponseDto>(medicalRecord);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecordResponseDto> UpdateAsync(int medicalRecordId, UpdateMedicalRecordRequestDto request, CancellationToken cancellationToken = default)
        {
            // Ensure the request object is not null.
            ArgumentNullException.ThrowIfNull(request);

            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the medical record identifier.
            // ---------------------------------------------------------------------
            ValidateMedicalRecordId(medicalRecordId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Log the beginning of the update operation.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "Doctor User {UserId} started updating medical record {MedicalRecordId}.",
                _currentUserService.UserId,
                medicalRecordId);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Validate the follow-up date.
            // ---------------------------------------------------------------------
            ValidateFollowUpDate(request.FollowUpDate);

            // ---------------------------------------------------------------------
            // STEP 4:
            // Retrieve the existing medical record.
            // ---------------------------------------------------------------------
            MedicalRecord medicalRecord = await GetMedicalRecordOrThrowAsync(medicalRecordId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 5:
            // Retrieve the related appointment.
            // ---------------------------------------------------------------------
            Appointment appointment = await ValidateAppointmentExistsAsync(medicalRecord.AppointmentId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 6:
            // Ensure the authenticated doctor owns this appointment.
            // ---------------------------------------------------------------------
            await ValidateDoctorOwnsAppointmentAsync(appointment,cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 7:
            // Map the updated values into the existing entity.
            // ---------------------------------------------------------------------
            _mapper.Map(request, medicalRecord);

            //// ---------------------------------------------------------------------
            //// STEP 8:
            //// Update the medical record.
            //// ---------------------------------------------------------------------
            //_medicalRecordRepository.Update(medicalRecord);

            // ---------------------------------------------------------------------
            // STEP 9:
            // Persist the changes.
            // ---------------------------------------------------------------------
            await _context.SaveChangesAsync(cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 10:
            // Reload the medical record with all related entities.
            // ---------------------------------------------------------------------
            medicalRecord = await _medicalRecordRepository.
                GetByIdWithDetailsAsync(medicalRecord.Id, cancellationToken) ?? throw new InvalidOperationException("Failed to reload the updated medical record.");

            // ---------------------------------------------------------------------
            // STEP 11:
            // Log successful update.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "Medical record {MedicalRecordId} updated successfully.",
                medicalRecord!.Id);

            // ---------------------------------------------------------------------
            // STEP 12:
            // Return the updated response DTO.
            // ---------------------------------------------------------------------
            return _mapper.Map<MedicalRecordResponseDto>(medicalRecord);

        }


        //======================================================================================================
        // Private methods used for business Validation

        /// <summary>
        /// Validates the medical record identifier.
        /// </summary>
        /// <param name="medicalRecordId">
        /// Medical record identifier.
        /// </param>
        /// <exception cref="BadRequestException">
        /// Thrown when the identifier is invalid.
        /// </exception>
        private static void ValidateMedicalRecordId(int medicalRecordId)
        {
            if (medicalRecordId <= 0)
            {
                throw new BadRequestException("Medical record ID must be greater than zero.");
            }
        }


        /// <summary>
        /// Retrieves a medical record or throws a <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="medicalRecordId">
        /// Medical record identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The medical record.
        /// </returns>
        private async Task<MedicalRecord> GetMedicalRecordOrThrowAsync(int medicalRecordId, CancellationToken cancellationToken)
        {
            MedicalRecord? medicalRecord = await _medicalRecordRepository.GetByIdAsync(medicalRecordId, cancellationToken);


            if (medicalRecord is null)
            {
                throw new NotFoundException($"Medical record with ID '{medicalRecordId}' was not found.");

            }

            return medicalRecord;
        }

        /// <summary>
        /// Validates that the appointment exists.
        /// </summary>
        /// <param name="appointmentId">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The appointment.
        /// </returns>
        private async Task<Appointment> ValidateAppointmentExistsAsync(int appointmentId, CancellationToken cancellationToken)
        {
            Appointment? appointment =
                await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId, cancellationToken);

            if (appointment is null)
            {
                throw new NotFoundException($"Appointment with ID '{appointmentId}' was not found.");

            }

            return appointment;
        }

        /// <summary>
        /// Ensures that the appointment does not already have a medical record.
        /// </summary>
        /// <param name="appointmentId">
        /// Appointment identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        private async Task ValidateMedicalRecordNotExistsAsync(int appointmentId, CancellationToken cancellationToken)
        {
            bool exists = await _medicalRecordRepository.ExistsByAppointmentAsync(appointmentId, cancellationToken);

            if (exists)
            {
                throw new ConflictException("A medical record already exists for this appointment.");

            }
        }

        /// <summary>
        /// Ensures that the appointment has been completed.
        /// </summary>
        /// <param name="appointment">
        /// Appointment entity.
        /// </param>
        private static void ValidateAppointmentCompleted(Appointment appointment)
        {
            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new ConflictException("Medical records can only be created for completed appointments.");

            }
        }

        /// <summary>
        /// Ensures that the authenticated doctor owns the appointment.
        /// </summary>
        /// <param name="appointment">
        /// Appointment entity.
        /// </param>
        /// <exception cref="ForbiddenException">
        /// Thrown when the appointment belongs to another doctor.
        /// </exception>
        private async Task ValidateDoctorOwnsAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            Doctor? doctor = await _doctorRepository.GetByUserIdAsync(_currentUserService.UserId, cancellationToken);

            if (doctor is null)
            {
                throw new ForbiddenException("The authenticated doctor profile was not found.");

            }

            if (appointment.DoctorId != doctor.Id)
            {
                _logger.LogWarning(
                    "Doctor {UserId} attempted to manage appointment {AppointmentId} that belongs to another doctor.",
                    _currentUserService.UserId,
                    appointment.Id);

                throw new ForbiddenException("You are not allowed to manage this medical record.");
            }
        }

        /// <summary>
        /// Validates the follow-up date.
        /// </summary>
        /// <param name="followUpDate">
        /// Follow-up date.
        /// </param>
        private static void ValidateFollowUpDate(DateOnly? followUpDate)
        {
            if (followUpDate.HasValue && followUpDate.Value < DateOnly.FromDateTime(DateTime.Today))

            {
                throw new BadRequestException("Follow-up date cannot be in the past.");
            }
        }
    }
}
