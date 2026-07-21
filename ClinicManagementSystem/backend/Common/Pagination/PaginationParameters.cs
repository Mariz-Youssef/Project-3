namespace ClinicManagementSystem.backend.Common.Pagination
{
    /// <summary>
    /// Represents pagination parameters supplied by the client.
    /// </summary>
    public class PaginationParameters
    {
        

        private int _pageNumber = PaginationDefaults.DefaultPageNumber;
        private int _pageSize = PaginationDefaults.DefaultPageSize;

        /// <summary>
        /// Gets or sets the current page number.
        /// Values less than or equal to zero are automatically replaced with the default page number.
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value <= 0 ? PaginationDefaults.DefaultPageNumber : value;
        }

        /// <summary>
        /// Gets or sets the number of records per page.
        /// Values less than or equal to zero are replaced with the default page size.
        /// Values greater than the maximum allowed page size are capped.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value <= 0)
                {
                    _pageSize = PaginationDefaults.DefaultPageSize;
                }
                else if (value > PaginationDefaults.MaxPageSize)
                {
                    _pageSize = PaginationDefaults.MaxPageSize;
                }
                else
                {
                    _pageSize = value;
                }
            }
        }
    }
}
