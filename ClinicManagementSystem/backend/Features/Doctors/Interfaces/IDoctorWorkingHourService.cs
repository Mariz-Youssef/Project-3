using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.WorkingHours;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Doctors.Interfaces
{
    public interface IDoctorWorkingHourService 
    {
        Task<PagedResult<WorkingHourResponse>> GetByDoctorAsync(int doctorId,PaginationParameters pagination,CancellationToken cancellationToken = default);
        Task<WorkingHourResponse?> GetByIdAsync(int id,CancellationToken cancellationToken = default);
        Task<WorkingHourResponse> CreateAsync(int doctorId,CreateWorkingHourRequest request,CancellationToken cancellationToken = default);
        Task<WorkingHourResponse> UpdateAsync(int doctorId, int id, UpdateWorkingHourRequest request,CancellationToken cancellationToken = default); 
        Task DeleteAsync(int doctorId,int id,CancellationToken cancellationToken = default);
    }
}
