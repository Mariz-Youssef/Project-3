namespace ClinicManagementSystem.backend.Features.Authentication.Interfaces
{
    public interface IUserService
    {
        Task<bool> ExistsAsync(int id,CancellationToken cancellationToken = default);
    }
}
