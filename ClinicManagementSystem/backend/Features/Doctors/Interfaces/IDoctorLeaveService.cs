using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Leaves;

namespace ClinicManagementSystem.backend.Features.Doctors.Interfaces;

public interface IDoctorLeaveService
{
    Task<PagedResult<LeaveResponse>> GetByDoctorAsync(int doctorId,PaginationParameters pagination,CancellationToken cancellationToken = default);
    Task<LeaveResponse?> GetByIdAsync(int doctorId, int id,CancellationToken cancellationToken = default);
    Task<LeaveResponse> CreateAsync(int doctorId,CreateLeaveRequest request,CancellationToken cancellationToken = default);
    Task<LeaveResponse> UpdateAsync(int doctorId,int id,UpdateLeaveRequest request,CancellationToken cancellationToken = default);
    Task DeleteAsync(int doctorId,int id,CancellationToken cancellationToken = default);
}