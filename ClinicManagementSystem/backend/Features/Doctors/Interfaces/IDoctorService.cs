using ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Responses;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Doctors.Interfaces
{
    public interface IDoctorService
    {
        Task<IReadOnlyList<DoctorResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<DoctorResponse?> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        Task<DoctorResponse> CreateAsync(CreateDoctorRequest request,CancellationToken cancellationToken = default);
        Task<DoctorResponse> UpdateAsync(int id, UpdateDoctorRequest request,CancellationToken cancellationToken = default);
        Task DeleteAsync(int id,CancellationToken cancellationToken = default);
        Task<IReadOnlyList<DoctorResponse>> GetByDepartmentAsync(int departmentId,CancellationToken cancellationToken = default);
        Task<IReadOnlyList<DoctorResponse>> GetBySpecializationAsync(string specialization,CancellationToken cancellationToken = default);
    }
}
