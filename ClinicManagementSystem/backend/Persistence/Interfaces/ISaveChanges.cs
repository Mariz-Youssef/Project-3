namespace ClinicManagementSystem.backend.Persistence.Interfaces
{
    /// <summary>
    /// Provides a method to persist changes to the database.
    /// </summary>
    public interface ISaveChanges
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
