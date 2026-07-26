using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Common.Pagination
{
    /// <summary>
    /// Provides extension methods for applying pagination
    /// to <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryablePaginationExtensions
    {
        /// <summary>
        /// Converts the source query into a paginated result.
        /// </summary>
        /// <typeparam name="T">
        /// The entity type.
        /// </typeparam>
        /// <param name="query">
        /// Source query.
        /// </param>
        /// <param name="parameters">
        /// Pagination parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        ///<returns>
        /// A <see cref="PagedResult{T}"/> containing the requested page of items
        /// and the corresponding pagination metadata.
        /// </returns>

        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, PaginationParameters parameters, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query);
            ArgumentNullException.ThrowIfNull(parameters);

            // Total number of records before pagination.
            int totalRecords = await query.CountAsync(cancellationToken);

            // Calculate the number of records to skip.
            int skip = (parameters.PageNumber - 1) * parameters.PageSize;

            // Retrieve only the requested page.
            IReadOnlyList<T> items = await query
                // Skip the records from the previous pages
                .Skip(skip)
                // Retrieve only the requested page size
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            // Calculate the total number of pages.
            int totalPages = (int)Math.Ceiling(totalRecords / (double)parameters.PageSize);


            return new PagedResult<T>
            {
                Items = items,

                pagination = new PaginationMetadata
                {
                    PageNumber = parameters.PageNumber,

                    PageSize = parameters.PageSize,

                    TotalRecords = totalRecords,

                    TotalPages = totalPages,

                    HasNextPage = parameters.PageNumber < totalPages && totalRecords > 0,

                    HasPreviousPage = parameters.PageNumber > 1 && totalRecords > 0

                }
            };
        }
    }
}
