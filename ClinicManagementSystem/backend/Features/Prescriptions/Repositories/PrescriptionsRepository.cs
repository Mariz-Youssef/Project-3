using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.Prescriptions.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Prescriptions.Repositories
{
    /// <summary>
    /// Provides prescription-specific database operations.
    /// </summary>
    public class PrescriptionsRepository : GenericRepository<Prescription>, IPrescriptionsRepository
    {

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PrescriptionsRepository"/> class.
        /// </summary>
        /// <param name="context">
        /// Application database context.
        /// </param>
        public PrescriptionsRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsForMedicalRecordAsync(int medicalRecordId, CancellationToken cancellationToken = default)
        {
            return await Query()
                .AnyAsync(prescription => prescription.MedicalRecordId == medicalRecordId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<PagedResult<Prescription>> GetAllPagedAsync(PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            return await QueryWithDetails()
                .AsNoTracking()
                .OrderByDescending(prescription => prescription.CreatedAt)
                .ToPagedResultAsync(pagination, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<Prescription?> GetByIdWithDetailsAsync(int prescriptionId, CancellationToken cancellationToken = default)
        {
            return await QueryWithDetails()
                .FirstOrDefaultAsync(prescription => prescription.Id == prescriptionId, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<PagedResult<Prescription>> GetDoctorPrescriptionsPagedAsync(int doctorId, PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            return await QueryWithDetails()
                .AsNoTracking()
                .Where(prescription =>
                    prescription.MedicalRecord
                        .Appointment
                        .DoctorId == doctorId)
                .OrderByDescending(prescription => prescription.CreatedAt)
                .ToPagedResultAsync(pagination, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<PagedResult<Prescription>> GetPatientPrescriptionsPagedAsync(int patientId, PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            return await QueryWithDetails()
               .AsNoTracking()
               .Where(prescription =>
                   prescription.MedicalRecord
                       .Appointment
                       .PatientId == patientId)
               .OrderByDescending(prescription => prescription.CreatedAt)
               .ToPagedResultAsync(pagination, cancellationToken);
        }

        /// <inheritdoc/>
        public IQueryable<Prescription> QueryWithDetails()
        {
            return Query()

                .Include(prescription => prescription.MedicalRecord)
                    .ThenInclude(record => record.Appointment)
                        .ThenInclude(appointment => appointment.Doctor)
                            .ThenInclude(doctor => doctor.User)

                .Include(prescription => prescription.MedicalRecord)
                    .ThenInclude(record => record.Appointment)
                        .ThenInclude(appointment => appointment.Patient)
                            .ThenInclude(patient => patient.User);
        }
    }
}
