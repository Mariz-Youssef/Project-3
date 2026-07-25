using AutoMapper;
using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Services.Interfaces;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;
using ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Responses;
using ClinicManagementSystem.backend.Features.Prescriptions.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Prescriptions.Services
{
    /// <summary>
    /// Provides business operations for managing prescriptions.
    /// </summary>
    public class PrescriptionsService:IPrescriptionsService
    {
        private readonly IPrescriptionsRepository _prescriptionRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly ILogger<PrescriptionsService> _logger;


        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PrescriptionsService"/> class.
        /// </summary>
        public PrescriptionsService(
            IPrescriptionsRepository prescriptionRepository,
            IMedicalRecordRepository medicalRecordRepository,
            IDoctorRepository doctorRepository,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IApplicationDbContext context,
            ILogger<PrescriptionsService> logger)
        {
            _prescriptionRepository = prescriptionRepository;
            _medicalRecordRepository = medicalRecordRepository;
            _doctorRepository = doctorRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<PrescriptionResponseDto> CreateAsync(CreatePrescriptionRequestDto request, CancellationToken cancellationToken = default)
        {
            // Ensure the request object is not null.
            ArgumentNullException.ThrowIfNull(request);

            // -----------------------------------------------------------------
            // STEP 1:
            // Log the request.
            // -----------------------------------------------------------------

            _logger.LogInformation(
                "Doctor User {UserId} started creating a prescription for medical record {MedicalRecordId}.",
                _currentUserService.UserId,
                request.MedicalRecordId);

            // -----------------------------------------------------------------
            // STEP 2:
            // Validate medicine information.
            // -----------------------------------------------------------------

            ValidateMedicineName(request.MedicineName);
            ValidateDosage(request.Dosage);
            ValidateFrequency(request.Frequency);
            ValidateDuration(request.Duration);

            // -----------------------------------------------------------------
            // STEP 3:
            // Validate Medical Record.
            // -----------------------------------------------------------------

            MedicalRecord medicalRecord = await ValidateMedicalRecordExistsAsync(request.MedicalRecordId, cancellationToken);

            // -----------------------------------------------------------------
            // STEP 4:
            // Ensure authenticated doctor owns the medical record.
            // -----------------------------------------------------------------

            await ValidateDoctorOwnsMedicalRecordAsync(medicalRecord, cancellationToken);

            // -----------------------------------------------------------------
            // STEP 5:
            // Map DTO to entity.
            // -----------------------------------------------------------------

            Prescription prescription = _mapper.Map<Prescription>(request);
            // -----------------------------------------------------------------
            // STEP 6:
            // Add prescription.
            // -----------------------------------------------------------------

            await _prescriptionRepository.AddAsync(prescription, cancellationToken);

            // -----------------------------------------------------------------
            // STEP 7:
            // Save changes.
            // -----------------------------------------------------------------

            await _context.SaveChangesAsync(cancellationToken);
            // -----------------------------------------------------------------
            // STEP 8:
            // Reload with navigation properties.
            // -----------------------------------------------------------------

            prescription =
                await _prescriptionRepository.GetByIdWithDetailsAsync(
                    prescription.Id,
                    cancellationToken)
                ?? throw new InvalidOperationException(
                    "Failed to reload the created prescription.");

            // -----------------------------------------------------------------
            // STEP 9:
            // Log success.
            // -----------------------------------------------------------------

            _logger.LogInformation(
                "Prescription {PrescriptionId} created successfully.",
                prescription.Id);

            // -----------------------------------------------------------------
            // STEP 10:
            // Return response.
            // -----------------------------------------------------------------

            return _mapper.Map<PrescriptionResponseDto>(prescription);

        }

        /// <inheritdoc/>
        public async Task<PrescriptionResponseDto> UpdateAsync(int prescriptionId, UpdatePrescriptionRequestDto request, CancellationToken cancellationToken = default)
        {
            // Ensure the request object is not null.
            ArgumentNullException.ThrowIfNull(request);

            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate prescription identifier.
            // ---------------------------------------------------------------------
            ValidatePrescriptionId(prescriptionId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Log request.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "Doctor User {UserId} started updating prescription {PrescriptionId}.",
                _currentUserService.UserId,
                prescriptionId);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Validate prescription information.
            // ---------------------------------------------------------------------
            ValidateMedicineName(request.MedicineName);
            ValidateDosage(request.Dosage);
            ValidateFrequency(request.Frequency);
            ValidateDuration(request.Duration);

            // ---------------------------------------------------------------------
            // STEP 4:
            // Retrieve the prescription.
            // ---------------------------------------------------------------------
            Prescription prescription = await GetPrescriptionOrThrowAsync(prescriptionId, cancellationToken);
            // ---------------------------------------------------------------------
            // STEP 5:
            // Retrieve the related medical record.
            // ---------------------------------------------------------------------
            MedicalRecord medicalRecord = await ValidateMedicalRecordExistsAsync(prescription.MedicalRecordId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 6:
            // Ensure authenticated doctor owns the medical record.
            // ---------------------------------------------------------------------
            await ValidateDoctorOwnsMedicalRecordAsync(medicalRecord, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 7:
            // Map updated values.
            // ---------------------------------------------------------------------
            _mapper.Map(request, prescription);

            // ---------------------------------------------------------------------
            // STEP 8:
            // Persist changes.
            // ---------------------------------------------------------------------
            await _context.SaveChangesAsync(cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 9:
            // Reload entity with navigation properties.
            // ---------------------------------------------------------------------
            prescription =
                await _prescriptionRepository.GetByIdWithDetailsAsync(
                    prescription.Id,
                    cancellationToken)
                ?? throw new InvalidOperationException(
                    "Failed to reload the updated prescription.");

            // ---------------------------------------------------------------------
            // STEP 10:
            // Log successful update.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "Prescription {PrescriptionId} updated successfully.",
                prescription.Id);

            // ---------------------------------------------------------------------
            // STEP 11:
            // Return updated DTO.
            // ---------------------------------------------------------------------
            return _mapper.Map<PrescriptionResponseDto>(
                prescription);
        }


        /// <inheritdoc/>
        public async Task DeleteAsync(int prescriptionId, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the prescription identifier.
            // ---------------------------------------------------------------------
            ValidatePrescriptionId(prescriptionId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Retrieve the prescription.
            // ---------------------------------------------------------------------
            Prescription prescription = await GetPrescriptionOrThrowAsync(prescriptionId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Delete the prescription.
            // ---------------------------------------------------------------------
            _prescriptionRepository.Delete(prescription);

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
                "Prescription {PrescriptionId} deleted successfully.",
                prescription.Id);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(int prescriptionId, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the prescription identifier.
            // ---------------------------------------------------------------------
            ValidatePrescriptionId(prescriptionId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Determine whether the prescription exists.
            // ---------------------------------------------------------------------
            return await _prescriptionRepository.ExistsAsync(prescriptionId, cancellationToken);

        }

        /// <inheritdoc/>
        public async Task<PagedResult<PrescriptionResponseDto>> GetAllAsync(PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Log the request.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "User {UserId} requested prescriptions.",
                _currentUserService.UserId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Start with the query including all related entities.
            // ---------------------------------------------------------------------
            IQueryable<Prescription> query =
                _prescriptionRepository
                    .QueryWithDetails()
                    .AsNoTracking();

            // ---------------------------------------------------------------------
            // STEP 3:
            // Apply role-based filtering.
            // ---------------------------------------------------------------------

            if (_currentUserService.IsInRole(RoleNames.Admin))
            {
                // Admin can view all prescriptions.
            }
            else if (_currentUserService.IsInRole(RoleNames.Doctor))
            {
                query = query.Where(p =>
                    p.MedicalRecord
                     .Appointment
                     .Doctor
                     .UserId ==
                     _currentUserService.UserId);
            }
            else if (_currentUserService.IsInRole(RoleNames.Patient))
            {
                query = query.Where(p =>
                    p.MedicalRecord
                     .Appointment
                     .Patient
                     .UserId ==
                     _currentUserService.UserId);
            }
            else
            {
                throw new ForbiddenException("You are not authorized to access prescriptions.");
                    
            }

            // ---------------------------------------------------------------------
            // STEP 4:
            // Order the prescriptions.
            // ---------------------------------------------------------------------
            query = query.OrderByDescending(p => p.CreatedAt);

            // ---------------------------------------------------------------------
            // STEP 5:
            // Apply pagination.
            // ---------------------------------------------------------------------
            PagedResult<Prescription> pagedPrescriptions = await query.ToPagedResultAsync(pagination, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 6:
            // Map to response DTOs.
            // ---------------------------------------------------------------------
            IReadOnlyList<PrescriptionResponseDto> prescriptions =
                _mapper.Map<IReadOnlyList<PrescriptionResponseDto>>(
                    pagedPrescriptions.Items);

            // ---------------------------------------------------------------------
            // STEP 7:
            // Return the paginated response.
            // ---------------------------------------------------------------------
            return new PagedResult<PrescriptionResponseDto>
            {
                Items = prescriptions,
                pagination = pagedPrescriptions.pagination
            };
        }

        /// <inheritdoc/>
        public async Task<PrescriptionResponseDto> GetByIdAsync(int prescriptionId, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the prescription identifier.
            // ---------------------------------------------------------------------
            ValidatePrescriptionId(prescriptionId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Log the request.
            // ---------------------------------------------------------------------
            _logger.LogInformation(
                "User {UserId} requested prescription {PrescriptionId}.",
                _currentUserService.UserId,
                prescriptionId);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Retrieve the prescription with all related entities.
            // ---------------------------------------------------------------------
            Prescription? prescription = await _prescriptionRepository.GetByIdWithDetailsAsync(prescriptionId, cancellationToken);

            if (prescription is null)
            {
                throw new NotFoundException($"Prescription with ID '{prescriptionId}' was not found.");

            }

            // ---------------------------------------------------------------------
            // STEP 4:
            // Apply role-based authorization.
            // ---------------------------------------------------------------------

            if (_currentUserService.IsInRole(RoleNames.Admin))
            {
                // Admin can access any prescription.
            }
            else if (_currentUserService.IsInRole(RoleNames.Doctor))
            {
                if (prescription.MedicalRecord.Appointment.Doctor.UserId !=
                    _currentUserService.UserId)
                {
                    throw new ForbiddenException("You are not allowed to access this prescription.");

                }
            }
            else if (_currentUserService.IsInRole(RoleNames.Patient))
            {
                if (prescription.MedicalRecord.Appointment.Patient.UserId !=
                    _currentUserService.UserId)
                {
                    throw new ForbiddenException("You are not allowed to access this prescription.");

                }
            }
            else
            {
                throw new ForbiddenException("You are not authorized to access prescriptions.");

            }

            // ---------------------------------------------------------------------
            // STEP 5:
            // Return the response DTO.
            // ---------------------------------------------------------------------

            return _mapper.Map<PrescriptionResponseDto>(prescription);
        }

       

        //============================================================================================

        /// <summary>
        /// Validates the prescription identifier.
        /// </summary>
        /// <param name="prescriptionId">
        /// Prescription identifier.
        /// </param>
        /// <exception cref="BadRequestException">
        /// Thrown when the identifier is invalid.
        /// </exception>
        private static void ValidatePrescriptionId(int prescriptionId)
        {
            if (prescriptionId <= 0)
            {
                throw new BadRequestException("Prescription ID must be greater than zero.");
            }
        }

        /// <summary>
        /// Retrieves a prescription or throws a
        /// <see cref="NotFoundException"/>.
        /// </summary>
        /// <param name="prescriptionId">
        /// Prescription identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The prescription.
        /// </returns>
        private async Task<Prescription> GetPrescriptionOrThrowAsync(int prescriptionId, CancellationToken cancellationToken)
        {
            Prescription? prescription = await _prescriptionRepository.GetByIdAsync(prescriptionId, cancellationToken);

            if (prescription is null)
            {
                throw new NotFoundException($"Prescription with ID '{prescriptionId}' was not found.");

            }

            return prescription;
        }


        /// <summary>
        /// Validates that the medical record exists.
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
        private async Task<MedicalRecord> ValidateMedicalRecordExistsAsync(int medicalRecordId, CancellationToken cancellationToken)
        {
            MedicalRecord? medicalRecord =
                await _medicalRecordRepository.GetByIdWithDetailsAsync(medicalRecordId, cancellationToken);

            if (medicalRecord is null)
            {
                throw new NotFoundException($"Medical record with ID '{medicalRecordId}' was not found.");

            }

            return medicalRecord;
        }

        /// <summary>
        /// Ensures that the authenticated doctor owns
        /// the specified medical record.
        /// </summary>
        /// <param name="medicalRecord">
        /// Medical record.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <exception cref="ForbiddenException">
        /// Thrown when the authenticated doctor does not
        /// own the medical record.
        /// </exception>
        private async Task ValidateDoctorOwnsMedicalRecordAsync(MedicalRecord medicalRecord, CancellationToken cancellationToken)

        {
            Doctor? doctor = await _doctorRepository.GetByUserIdAsync(_currentUserService.UserId, cancellationToken);

            if (doctor is null)
            {
                throw new ForbiddenException("The authenticated doctor profile was not found.");

            }

            if (medicalRecord.Appointment.DoctorId != doctor.Id)
            {
                throw new ForbiddenException("You are not allowed to manage prescriptions for this medical record.");

            }
        }

        /// <summary>
        /// Validates the medicine name.
        /// </summary>
        /// <param name="medicineName">
        /// Medicine name.
        /// </param>
        private static void ValidateMedicineName(string medicineName)
        {
            if (string.IsNullOrWhiteSpace(medicineName))
            {
                throw new BadRequestException("Medicine name is required.");

            }
        }

        /// <summary>
        /// Validates the dosage.
        /// </summary>
        /// <param name="dosage">
        /// Dosage.
        /// </param>
        private static void ValidateDosage(string dosage)
        {
            if (string.IsNullOrWhiteSpace(dosage))
            {
                throw new BadRequestException("Dosage is required.");

            }
        }

        /// <summary>
        /// Validates the frequency.
        /// </summary>
        /// <param name="frequency">
        /// Frequency.
        /// </param>
        private static void ValidateFrequency(string frequency)
        {
            if (string.IsNullOrWhiteSpace(frequency))
            {
                throw new BadRequestException("Frequency is required.");

            }
        }

        /// <summary>
        /// Validates the treatment duration.
        /// </summary>
        /// <param name="duration">
        /// Treatment duration.
        /// </param>
        private static void ValidateDuration(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
            {
                throw new BadRequestException("Duration is required.");

            }
        }


    }
}
