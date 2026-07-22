using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.Patients.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Patients.Repositories;

public sealed class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Patient?> GetPatientProfileWithUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Include(patient => patient.User)
            .FirstOrDefaultAsync(patient => patient.UserId == userId, cancellationToken);
    }

    public async Task<Patient?> GetPatientByIdWithUserAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Include(patient => patient.User)
            .FirstOrDefaultAsync(patient => patient.Id == patientId, cancellationToken);
    }

    public async Task<IEnumerable<Patient>> SearchPatientsWithUserAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .Include(patient => patient.User)
            .Where(patient => patient.User.FullName.Contains(searchTerm) || patient.EmergencyContactPhone.Contains(searchTerm))
            .OrderBy(patient => patient.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}