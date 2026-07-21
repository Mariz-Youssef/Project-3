namespace ClinicManagementSystem.backend.Common.Pagination
{
    /// <summary>
    /// Defines the default pagination settings used throughout the application.
    /// </summary>
    public static class PaginationDefaults
    {
        /// <summary>
        /// Default page number.
        /// </summary>
        public const int DefaultPageNumber = 1;

        /// <summary>
        /// Default number of records per page.
        /// </summary>
        public const int DefaultPageSize = 5;

        /// <summary>
        /// Maximum number of records allowed per page.
        /// </summary>
        public const int MaxPageSize = 100;
    }
}
