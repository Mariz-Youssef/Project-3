using ClinicManagementSystem.backend.Models;
using System.Linq.Expressions;

namespace ClinicManagementSystem.backend.Persistence.Interfaces
{
    /// <summary>
    /// Generic repository interface that provides common CRUD operations
    /// for all entities.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>

    //If the client closes the browser before the request finishes,the request is cancelled immediately.
    //This saves SQL Server resources.
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        #region Read Operations

        /// <summary>
        /// Returns all entities.
        /// </summary>
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns an entity by its Id.
        /// </summary>
        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all entities matching the specified condition.
        /// </summary>
        Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the first entity matching the specified condition.
        /// </summary>
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether an entity exists by Id.
        /// </summary>
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether any entity matches the specified condition.
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the total number of entities.
        /// </summary>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the number of entities matching the specified condition.
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns IQueryable for filtering, sorting and pagination.
        /// </summary>
        IQueryable<TEntity> Query();

        #endregion

        #region Write Operations

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple entities.
        /// </summary>
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes multiple entities.
        /// </summary>
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        #endregion
    }
}