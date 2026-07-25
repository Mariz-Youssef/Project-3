using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Enums;
using ClinicManagementSystem.backend.Features.Appointments.DTOs.Responses;
using ClinicManagementSystem.backend.Features.Appointments.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Appointments.Repositories
{   
    /// <summary>
    /// Provides appointment-specific data access operations.
    /// </summary>
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentRepository"/> class.
        /// </summary>
        /// <param name="context">
        /// Application database context.
        /// </param>
        public AppointmentRepository(ApplicationDbContext context ) : base(context)
        {
            
        }

       

        /// <inheritdoc/>
        public async Task<Appointment> GetDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(appointment => appointment.Doctor)
                    .ThenInclude(doctor => doctor.User)
                .Include(appointment => appointment.Patient)
                    .ThenInclude(patient => patient.User)
                .Include(appointment => appointment.MedicalRecord)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    appointment => appointment.Id == id,
                    cancellationToken);

        }


        /// <inheritdoc/>
        public IQueryable<Appointment> QueryWithDetails()
        {
            return _dbSet
                .Include(appointment => appointment.Doctor)
                    .ThenInclude(doctor => doctor.User)
                .Include(appointment => appointment.Patient)
                    .ThenInclude(patient => patient.User)
                .Include(appointment => appointment.MedicalRecord)
                .AsNoTracking();
        }

        /// <inheritdoc/>
        public async Task<Appointment?> GetByIdWithDetailsAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.User)
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Department)
                .Include(a => a.Patient)
                    .ThenInclude(p => p.User)
                .Include(a => a.MedicalRecord)
                .FirstOrDefaultAsync(
                    a => a.Id == appointmentId,
                    cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> DoctorHasOverlappingAppointmentAsync(int doctorId, DateOnly appointmentDate, TimeOnly startTime, TimeOnly endTime, int? excludeAppointmentId = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.AppointmentDate == appointmentDate);

            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(a => a.Id != excludeAppointmentId.Value);
            }

            return await query.AnyAsync(
                a => startTime < a.EndTime &&
                     endTime > a.StartTime,
                cancellationToken);
        }


        /// <inheritdoc/>
        public async Task<bool> PatientHasOverlappingAppointmentAsync(int patientId, DateOnly appointmentDate, TimeOnly startTime, TimeOnly endTime, int? excludeAppointmentId = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Where(a =>
                    a.PatientId == patientId &&
                    a.AppointmentDate == appointmentDate);

            if (excludeAppointmentId.HasValue)
            {
                query = query.Where(a => a.Id != excludeAppointmentId.Value);
            }

            return await query.AnyAsync(
                a => startTime < a.EndTime &&
                     endTime > a.StartTime,
                cancellationToken);
        }

        public async Task<List<Appointment>> GetDoctorAppointmentsByDateAsync(int doctorId, DateOnly date, CancellationToken cancellationToken)
        {
            return await _context.Appointments.Where(a =>
              a.DoctorId == doctorId &&
              a.AppointmentDate == date &&
              a.Status != AppointmentStatus.Cancelled)
             .ToListAsync(cancellationToken);
        }

    }
}
