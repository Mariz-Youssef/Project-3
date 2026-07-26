namespace ClinicManagementSystem.backend.Common.Pagination
{
    /// <summary>
    /// Represents a paginated result returned from the data source.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the paginated items.
    /// </typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Gets or initializes the paginated items.
        /// </summary>
        public IReadOnlyList<T> Items { get; init; } = [];

        /// <summary>
        /// Gets or initializes the pagination metadata.
        /// </summary>
        public PaginationMetadata pagination { get; init; } = default!;

    }
}
