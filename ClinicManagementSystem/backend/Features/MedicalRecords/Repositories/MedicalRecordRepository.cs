using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Repositories
{
    /// <summary>
    /// Provides medical record-specific database operations.
    /// </summary>
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsByAppointmentAsync(int appointmentId, CancellationToken cancellationToken = default)
        {
            return await Query()
                 .AnyAsync(
                     record => record.AppointmentId == appointmentId,
                     cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<PagedResult<MedicalRecord>> GetAllPagedAsync(PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            return await QueryWithDetails()
                .AsNoTracking()
                .OrderByDescending(record => record.CreatedAt)
                .ToPagedResultAsync(pagination, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<MedicalRecord?> GetByIdWithDetailsAsync(int medicalRecordId, CancellationToken cancellationToken = default)
        {
            return await QueryWithDetails()
                .FirstOrDefaultAsync(record => record.Id == medicalRecordId, cancellationToken);

        }

        /// <inheritdoc/>
        public async Task<PagedResult<MedicalRecord>> GetDoctorRecordsPagedAsync(int doctorId, PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            return await QueryWithDetails()
               .AsNoTracking()
               .Where(record => record.Appointment.DoctorId == doctorId)
               .OrderByDescending(record => record.CreatedAt)
               .ToPagedResultAsync(pagination, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<PagedResult<MedicalRecord>> GetPatientRecordsPagedAsync(int patientId, PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            return await QueryWithDetails()
                .AsNoTracking()
                .Where(record => record.Appointment.PatientId == patientId)
                .OrderByDescending(record => record.CreatedAt)
                .ToPagedResultAsync(pagination, cancellationToken);
        }

        /// <inheritdoc/>
        public IQueryable<MedicalRecord> QueryWithDetails()
        {
            return Query()

                .Include(record => record.Appointment)
                    .ThenInclude(appointment => appointment.Doctor)
                        .ThenInclude(doctor => doctor.User)

                .Include(record => record.Appointment)
                    .ThenInclude(appointment => appointment.Patient)
                        .ThenInclude(patient => patient.User)

                .Include(record => record.Prescriptions);
        }
    }
}
