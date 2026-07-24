/*using Microsoft.EntityFrameworkCore;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.MedicalRecords.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence;
using ClinicManagementSystem.backend.Persistence.Repositories;
using ClinicManagementSystem.backend.Data;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Repositories;

/// <summary>
/// Implements medical-record-specific data access operations.
/// </summary>
public class MedicalRecordRepository : GenericRepository<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<MedicalRecord?> GetByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<MedicalRecord>()
            .AsNoTracking()
            .FirstOrDefaultAsync(mr => mr.AppointmentId == appointmentId, cancellationToken);
    }

    public async Task<PagedResult<MedicalRecord>> GetPatientHistoryAsync(int patientId, PaginationParameters pagination, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<MedicalRecord>()
            .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Doctor)
                    .ThenInclude(d => d.User)
            .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
                    .ThenInclude(p => p.User)
            .Where(mr => mr.Appointment.PatientId == patientId)
            .OrderByDescending(mr => mr.CreatedAt)
            .AsNoTracking();

        return await query.ToPagedResultAsync(pagination, cancellationToken);
    }

    public async Task<PagedResult<MedicalRecord>> GetPatientHistoryByDoctorAsync(int patientId, int doctorId, PaginationParameters pagination, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<MedicalRecord>()
            .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Doctor)
                    .ThenInclude(d => d.User)
            .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
                    .ThenInclude(p => p.User)
            .Where(mr => mr.Appointment.PatientId == patientId && mr.Appointment.DoctorId == doctorId)
            .OrderByDescending(mr => mr.CreatedAt)
            .AsNoTracking();

        return await query.ToPagedResultAsync(pagination, cancellationToken);
    }

    public async Task<MedicalRecord?> GetByIdWithDetailsAsync(int medicalRecordId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<MedicalRecord>()
            .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Doctor)
                    .ThenInclude(d => d.User)
            .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
                    .ThenInclude(p => p.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(mr => mr.Id == medicalRecordId, cancellationToken);
    }

    public async Task<MedicalRecord?> GetByIdForUpdateAsync(int medicalRecordId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<MedicalRecord>()
            .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Doctor)
                    .ThenInclude(d => d.User)
            .Include(mr => mr.Appointment)
                .ThenInclude(a => a.Patient)
            .FirstOrDefaultAsync(mr => mr.Id == medicalRecordId, cancellationToken);
    }
}*/