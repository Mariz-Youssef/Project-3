namespace ClinicManagementSystem.backend.Common.Pagination
{
    /// <summary>
    /// Represents pagination information returned
    /// with paginated API responses.
    /// </summary>
    public class PaginationMetadata
    {
        /// <summary>
        /// Current page number.
        /// </summary>
        public int PageNumber { get; init; }

        /// <summary>
        /// Number of records per page.
        /// </summary>
        public int PageSize { get; init; }

        /// <summary>
        /// Total number of pages.
        /// </summary>
        public int TotalPages { get; init; }

        /// <summary>
        /// Total number of records.
        /// </summary>
        public int TotalRecords { get; init; }

        /// <summary>
        /// Indicates whether another page exists.
        /// </summary>
        public bool HasNextPage { get; init; }

        /// <summary>
        /// Indicates whether a previous page exists.
        /// </summary>
        public bool HasPreviousPage { get; init; }
    }
}
