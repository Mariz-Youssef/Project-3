using AutoMapper;
using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Common.Services.Interfaces;
using ClinicManagementSystem.backend.Enums;
using ClinicManagementSystem.backend.Features.Appointments.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Appointments.DTOs.Responses;
using ClinicManagementSystem.backend.Features.Appointments.Interfaces;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Appointments.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorWorkingHourRepository _workingHourRepository;
        private readonly IDoctorLeaveRepository _leaveRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        //inject CurrentUserService
        private readonly ICurrentUserService _currentUserService;

        //Constructor
        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IDoctorWorkingHourRepository workingHourRepository,
            IDoctorLeaveRepository leaveRepository,
            IPatientRepository patientRepository,
            IApplicationDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _workingHourRepository = workingHourRepository;
            _leaveRepository = leaveRepository;
            _patientRepository = patientRepository;
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        /// <inheritdoc/>
        public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentRequestDto request, CancellationToken cancellationToken = default)
        {
            // Ensure the request object is not null.
            ArgumentNullException.ThrowIfNull(request);

            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the appointment date and time.
            // This prevents creating appointments in the past or with invalid times.
            // ---------------------------------------------------------------------
            ValidateAppointmentDate(request.AppointmentDate);

            ValidateAppointmentTime(request.StartTime, request.EndTime);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Retrieve the authenticated user's identifier.
            // Since only authenticated patients are allowed to book appointments,
            // this UserId belongs to an ApplicationUser.
            // ---------------------------------------------------------------------
            int userId = _currentUserService.UserId;

            // ---------------------------------------------------------------------
            // STEP 3:
            // Retrieve the patient's profile associated with the authenticated user.
            // Throws NotFoundException if the patient profile does not exist.
            // ---------------------------------------------------------------------
            Patient patient = await ValidatePatientExistsAsync(userId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 4:
            // Retrieve the requested doctor.
            // Throws NotFoundException if the doctor does not exist.
            // ---------------------------------------------------------------------
            Doctor doctor = await ValidateDoctorExistsAsync(request.DoctorId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 5:
            // Ensure the doctor works during the requested day and time.
            // ---------------------------------------------------------------------
            await ValidateDoctorWorkingHoursAsync(doctor.Id, request.AppointmentDate, request.StartTime, request.EndTime, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 6:
            // Ensure the doctor is not on leave.
            // ---------------------------------------------------------------------
            await ValidateDoctorLeaveAsync(doctor.Id, request.AppointmentDate, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 7:
            // Ensure the doctor does not already have another appointment
            // during the requested time.
            // excludeAppointmentId = null because this is a Create operation.
            // ---------------------------------------------------------------------
            await ValidateDoctorAppointmentConflictAsync(doctor.Id, request.AppointmentDate, request.StartTime, request.EndTime, null, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 8:
            // Ensure the patient does not already have another appointment
            // during the requested time.
            // excludeAppointmentId = null because this is a Create operation.
            // ---------------------------------------------------------------------
            await ValidatePatientAppointmentConflictAsync(patient.Id, request.AppointmentDate, request.StartTime, request.EndTime, null, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 9:
            // Map the request DTO to a new Appointment entity.
            // ---------------------------------------------------------------------
            Appointment appointment = _mapper.Map<Appointment>(request);

            // ---------------------------------------------------------------------
            // STEP 10:
            // Assign server-controlled properties.
            // The patient is determined from the authenticated user,
            // not from the client request.
            // ---------------------------------------------------------------------

            // Every newly created appointment starts as Pending.
            appointment.Status = AppointmentStatus.Pending;
            appointment.PatientId = patient.Id;

            // ---------------------------------------------------------------------
            // STEP 11:
            // Add the appointment to the repository.
            // ---------------------------------------------------------------------
            await _appointmentRepository.AddAsync(appointment, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 12:
            // Persist the appointment in the database.
            // ---------------------------------------------------------------------
            await _context.SaveChangesAsync(cancellationToken);

            appointment = await _appointmentRepository.GetByIdWithDetailsAsync(appointment.Id, cancellationToken);

            if (appointment is null)
            {
                throw new NotFoundException("The created appointment could not be loaded.");
            }

            // ---------------------------------------------------------------------
            // STEP 13:
            // Return the created appointment as a response DTO.
            // ---------------------------------------------------------------------
            return _mapper.Map<AppointmentResponseDto>(appointment);

        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the appointment identifier.
            // ---------------------------------------------------------------------
            ValidateAppointmentId(appointmentId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Retrieve the appointment.
            // Throws NotFoundException if it does not exist.
            // ---------------------------------------------------------------------
            Appointment appointment = await GetAppointmentOrThrowAsync(appointmentId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Prevent deleting completed appointments.
            // Completed appointments become part of the patient's medical history.
            // ---------------------------------------------------------------------
            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new ConflictException("Completed appointments cannot be deleted.");

            }

            // ---------------------------------------------------------------------
            // STEP 4:
            // Mark the appointment as deleted.
            // The Global Soft Delete Infrastructure will hide it automatically.
            // ---------------------------------------------------------------------
            _appointmentRepository.Delete(appointment);

            // ---------------------------------------------------------------------
            // STEP 5:
            // Persist the changes.
            // ---------------------------------------------------------------------
            await _context.SaveChangesAsync(cancellationToken);

        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the appointment identifier.
            // ---------------------------------------------------------------------
            ValidateAppointmentId(appointmentId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Check whether the appointment exists.
            // ---------------------------------------------------------------------
            return await _appointmentRepository.ExistsAsync(appointmentId, cancellationToken);

        }

        /// <inheritdoc/>
        public async Task<PagedResult<AppointmentResponseDto>> GetAllAsync(PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Ensure pagination parameters are provided.
            // ---------------------------------------------------------------------
            ArgumentNullException.ThrowIfNull(pagination);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Build the query with all required related entities.
            // The appointments are ordered by appointment date then start time.
            // ---------------------------------------------------------------------
            IQueryable<Appointment> query = _appointmentRepository
                .QueryWithDetails()
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Apply pagination.
            // ---------------------------------------------------------------------
            PagedResult<Appointment> pagedAppointments = await query.ToPagedResultAsync(pagination, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 4:
            // Convert entities to response DTOs.
            // ---------------------------------------------------------------------
            return new PagedResult<AppointmentResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<AppointmentResponseDto>>(
                    pagedAppointments.Items),

                pagination = pagedAppointments.pagination
            };

        }

        /// <inheritdoc/>
        public async Task<AppointmentDetailsResponseDto> GetByIdAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the appointment identifier.
            // ---------------------------------------------------------------------
            ValidateAppointmentId(appointmentId);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Retrieve the appointment with all related data.
            // ---------------------------------------------------------------------
            Appointment? appointment = await GetAppointmentWithDetailsOrThrowAsync(appointmentId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Throw an exception if the appointment does not exist.
            // ---------------------------------------------------------------------
            if (appointment is null)
            {
                throw new NotFoundException($"Appointment with ID '{appointmentId}' was not found.");

            }

            // ---------------------------------------------------------------------
            // STEP 4:
            // Map the entity to the response DTO.
            // ---------------------------------------------------------------------
            return _mapper.Map<AppointmentDetailsResponseDto>(appointment);

        }

        /// <inheritdoc/>
        public async Task<AppointmentResponseDto> UpdateAsync(int appointmentId, UpdateAppointmentRequestDto request, CancellationToken cancellationToken = default)
        {
            // ---------------------------------------------------------------------
            // STEP 1:
            // Validate the input parameters.
            // ---------------------------------------------------------------------
            ValidateAppointmentId(appointmentId);

            ArgumentNullException.ThrowIfNull(request);

            // ---------------------------------------------------------------------
            // STEP 2:
            // Retrieve the existing appointment.
            // Throws NotFoundException if it does not exist.
            // ---------------------------------------------------------------------
            Appointment appointment = await GetAppointmentOrThrowAsync(appointmentId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 3:
            // Validate the new appointment date and time.
            // ---------------------------------------------------------------------
            ValidateAppointmentDate(request.AppointmentDate);

            ValidateAppointmentTime(request.StartTime, request.EndTime);

            // ---------------------------------------------------------------------
            // STEP 4:
            // Ensure the doctor assigned to this appointment still exists.
            // The doctor cannot be changed during update.
            // ---------------------------------------------------------------------
            await ValidateDoctorExistsAsync(appointment.DoctorId, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 5:
            // Ensure the doctor works during the requested day and time.
            // ---------------------------------------------------------------------
            await ValidateDoctorWorkingHoursAsync(appointment.DoctorId, request.AppointmentDate, request.StartTime, request.EndTime, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 6:
            // Ensure the doctor is not on leave.
            // ---------------------------------------------------------------------
            await ValidateDoctorLeaveAsync(appointment.DoctorId, request.AppointmentDate, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 7:
            // Ensure the doctor has no conflicting appointments.
            // Exclude the current appointment from the validation.
            // ---------------------------------------------------------------------
            await ValidateDoctorAppointmentConflictAsync(appointment.DoctorId, request.AppointmentDate, request.StartTime, request.EndTime, appointment.Id, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 8:
            // Ensure the patient has no conflicting appointments.
            // Exclude the current appointment from the validation.
            // ---------------------------------------------------------------------
            await ValidatePatientAppointmentConflictAsync(appointment.PatientId, request.AppointmentDate, request.StartTime, request.EndTime, appointment.Id, cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 9:
            // Update only editable properties.
            // DoctorId, PatientId and Status remain unchanged.
            // ---------------------------------------------------------------------
            _mapper.Map(request, appointment);

            // ---------------------------------------------------------------------
            // STEP 10:
            // Update the modification timestamp.
            // ---------------------------------------------------------------------
            appointment.UpdatedAt = DateTime.UtcNow;

            // ---------------------------------------------------------------------
            // STEP 11:
            // Mark the entity as modified.
            // ---------------------------------------------------------------------
            _appointmentRepository.Update(appointment);

            // ---------------------------------------------------------------------
            // STEP 12:
            // Persist the changes.
            // ---------------------------------------------------------------------
            await _context.SaveChangesAsync(cancellationToken);

            // ---------------------------------------------------------------------
            // STEP 13:
            // Reload the appointment with related entities.
            // ---------------------------------------------------------------------
            appointment = await _appointmentRepository.GetByIdWithDetailsAsync(appointment.Id, cancellationToken) ?? throw new NotFoundException("The updated appointment could not be loaded.");

            // ---------------------------------------------------------------------
            // STEP 14:
            // Convert the updated entity to a response DTO.
            // ---------------------------------------------------------------------
            return _mapper.Map<AppointmentResponseDto>(appointment);

        }

        //=================================================================================================

        //Private methods used for business validation


        /// <summary>
        /// Validates the appointment identifier.
        /// </summary>
        /// <param name="appointmentId">
        /// Appointment identifier.
        /// </param>
        /// <exception cref="BadRequestException">
        /// Thrown when the identifier is invalid.
        /// </exception>
        private static void ValidateAppointmentId(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new BadRequestException("Appointment ID must be greater than zero.");
            }
        }

        /// <summary>
        /// Retrieves an appointment or throws a <see cref="NotFoundException"/>.
        /// </summary>
        private async Task<Appointment> GetAppointmentOrThrowAsync(int appointmentId, CancellationToken cancellationToken)
        {
            Appointment? appointment = await _appointmentRepository.GetByIdAsync(appointmentId, cancellationToken);

            if (appointment is null)
            {
                throw new NotFoundException($"Appointment with ID '{appointmentId}' was not found.");

            }

            return appointment;
        }

        /// <summary>
        /// Validates the appointment date no in the past.
        /// </summary>
        private static void ValidateAppointmentDate(DateOnly appointmentDate)
        {
            if (appointmentDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new BadRequestException("Appointment date cannot be in the past.");

            }
        }

        /// <summary>
        /// Validates appointment start and end times (End time must be greater than start time).
        /// </summary>
        private static void ValidateAppointmentTime(TimeOnly startTime, TimeOnly endTime)
        {
            if (endTime <= startTime)
            {
                throw new BadRequestException("End time must be greater than start time.");

            }
        }

        /// <summary>
        /// Validates that the doctor exists.
        /// </summary>
        private async Task<Doctor> ValidateDoctorExistsAsync(int doctorId, CancellationToken cancellationToken)
        {
            Doctor? doctor = await _doctorRepository.GetDoctorForAppointmentAsync(doctorId, cancellationToken);

            if (doctor is null)
            {
                throw new NotFoundException($"Doctor with ID '{doctorId}' was not found.");

            }

            return doctor;
        }

        /// <summary>
        /// Validates that the patient exists based on his profile.
        /// </summary>
        private async Task<Patient> ValidatePatientExistsAsync(int userId, CancellationToken cancellationToken)
        {
            Patient? patient = await _patientRepository.GetByUserIdAsync(userId, cancellationToken);

            if (patient is null)
            {
                throw new NotFoundException("Patient profile was not found.");
            }

            return patient;
        }

        /// <summary>
        /// Ensures that the doctor works during the requested period.
        /// </summary>
        private async Task ValidateDoctorWorkingHoursAsync(int doctorId, DateOnly appointmentDate, TimeOnly startTime, TimeOnly endTime, CancellationToken cancellationToken)

        {
            DayOfWeek day = appointmentDate.DayOfWeek;

            DoctorWorkingHour? workingHours = await _workingHourRepository.GetWorkingHoursAsync(doctorId, day, cancellationToken);

            if (workingHours is null)
            {
                throw new ConflictException("The doctor does not work on the selected day.");

            }

            if (startTime < workingHours.StartTime ||
                endTime > workingHours.EndTime)
            {
                throw new ConflictException("The appointment is outside the doctor's working hours.");

            }
        }

        /// <summary>
        /// Ensures that the doctor is not on leave.
        /// </summary>
        private async Task ValidateDoctorLeaveAsync(int doctorId, DateOnly appointmentDate, CancellationToken cancellationToken)
        {
            bool isOnLeave = await _leaveRepository.IsOnLeaveAsync(doctorId, appointmentDate, cancellationToken);

            if (isOnLeave)
            {
                throw new ConflictException("The doctor is on leave on the selected date.");

            }
        }


        /// <summary>
        /// Ensures that the doctor has no overlapping appointment.
        /// </summary>
        private async Task ValidateDoctorAppointmentConflictAsync(int doctorId, DateOnly appointmentDate, TimeOnly startTime, TimeOnly endTime, int? excludeAppointmentId, CancellationToken cancellationToken)

        {
            bool hasConflict = await _appointmentRepository.DoctorHasOverlappingAppointmentAsync(doctorId, appointmentDate, startTime, endTime, excludeAppointmentId, cancellationToken);

            if (hasConflict)
            {
                throw new ConflictException("The doctor already has another appointment during the selected time.");

            }
        }



        /// <summary>
        /// Ensures that the patient has no overlapping appointment.
        /// </summary>
        private async Task ValidatePatientAppointmentConflictAsync(int patientId, DateOnly appointmentDate, TimeOnly startTime, TimeOnly endTime, int? excludeAppointmentId, CancellationToken cancellationToken)
        {
            bool hasConflict = await _appointmentRepository.PatientHasOverlappingAppointmentAsync(patientId, appointmentDate, startTime, endTime, excludeAppointmentId, cancellationToken);

            if (hasConflict)
            {
                throw new ConflictException("The patient already has another appointment during the selected time.");

            }
        }



        /// <summary>
        /// Retrieves an appointment with all related entities.
        /// </summary>
        private async Task<Appointment> GetAppointmentWithDetailsOrThrowAsync(int appointmentId, CancellationToken cancellationToken)
        {
            Appointment? appointment = await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId, cancellationToken);

            if (appointment is null)
            {
                throw new NotFoundException($"Appointment with ID '{appointmentId}' was not found.");

            }

            return appointment;
        }
        public async Task<AppointmentResponseDto> ConfirmAsync(int appointmentId,CancellationToken cancellationToken = default)
        {
            var appointment = await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId, cancellationToken);

            if (appointment is null)
                throw new NotFoundException("Appointment not found.");

            EnsureCanConfirm(appointment);
            appointment.Status = AppointmentStatus.Confirmed;
            _appointmentRepository.Update(appointment);

            await _context.SaveChangesAsync(cancellationToken);

            var updatedAppointment = await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId,cancellationToken);
            return _mapper.Map<AppointmentResponseDto>(updatedAppointment);
        }
        public async Task<AppointmentResponseDto> CompleteAsync(int appointmentId,CancellationToken cancellationToken = default)
        {
            var appointment = await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId, cancellationToken);

            if (appointment is null)
                throw new NotFoundException("Appointment not found.");

            EnsureCanComplete(appointment);

            appointment.Status = AppointmentStatus.Completed;

            _appointmentRepository.Update(appointment);

            await _context.SaveChangesAsync(cancellationToken);

            var updatedAppointment = await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId,cancellationToken);

            return _mapper.Map<AppointmentResponseDto>(updatedAppointment);
        }
        public async Task<AppointmentResponseDto> CancelAsync(int appointmentId,CancellationToken cancellationToken = default)
        {
            var appointment = await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId, cancellationToken);

            if (appointment is null)
                throw new NotFoundException("Appointment not found.");

            EnsureCanCancel(appointment);
            appointment.Status = AppointmentStatus.Cancelled;

            _appointmentRepository.Update(appointment);

            await _context.SaveChangesAsync(cancellationToken);

            var updatedAppointment =
                await _appointmentRepository.GetByIdWithDetailsAsync(appointmentId,cancellationToken);

            return _mapper.Map<AppointmentResponseDto>(updatedAppointment);
        }
        //helper methods

        private static void EnsureCanConfirm(Appointment appointment)
        {
            if (appointment.Status != AppointmentStatus.Pending)
            {
                throw new ConflictException(
                    "Only pending appointments can be confirmed.");
            }
        }

        private static void EnsureCanComplete(Appointment appointment)
        {
            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                throw new ConflictException(
                    "Only confirmed appointments can be completed.");
            }
        }

        private static void EnsureCanCancel(Appointment appointment)
        {
            if (appointment.Status is AppointmentStatus.Completed or AppointmentStatus.Cancelled)
            {
                throw new ConflictException(
                    "Completed or cancelled appointments cannot be cancelled.");
            }
        }
        public async Task<IEnumerable<AvailableSlotResponse>> GetAvailableSlotsAsync(int doctorId, DateOnly date, CancellationToken cancellationToken)
        {
            var workingHours = await _workingHourRepository.GetByDoctorAsync(
                doctorId,
                cancellationToken);

            var schedule = workingHours.FirstOrDefault(w =>
                w.DayOfWeek == date.DayOfWeek);

            if (schedule is null)
                return Enumerable.Empty<AvailableSlotResponse>();

            var appointments = await _appointmentRepository
                .GetDoctorAppointmentsByDateAsync(
                    doctorId,
                    date,
                    cancellationToken);

            var bookedSlots = appointments
                .Select(a => a.StartTime)
                .ToHashSet();

            var availableSlots = new List<AvailableSlotResponse>();

            var currentSlot = schedule.StartTime;

            while (currentSlot.AddMinutes(AppointmentConstants.AppointmentDurationMinutes)
                   <= schedule.EndTime)
            {
                if (!bookedSlots.Contains(currentSlot))
                {
                    availableSlots.Add(new AvailableSlotResponse
                    {
                        Time = currentSlot
                    });
                }

                currentSlot = currentSlot.AddMinutes(
                    AppointmentConstants.AppointmentDurationMinutes);
            }

            return availableSlots;
        }
    }
}
