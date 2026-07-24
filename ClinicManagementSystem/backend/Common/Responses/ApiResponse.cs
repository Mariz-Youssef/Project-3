using ClinicManagementSystem.backend.Common.Pagination;

namespace ClinicManagementSystem.backend.Common.Responses
{
    /// <summary>
    /// Represents the standard API response returned by all endpoints.
    /// Provides a consistent structure for success, failure, validation,
    /// and paginated responses.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the response data.
    /// </typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the request was processed successfully.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Human-readable message describing the result.
        /// </summary>
        public string Message { get; init; } = string.Empty;

        /// <summary>
        /// Response payload.
        /// Returns null when the request fails.
        /// </summary>
        public T? Data { get; init; }

        /// <summary>
        /// Collection of validation or error messages.
        /// Null for successful responses.
        /// </summary>
        public IReadOnlyList<string>? Errors { get; init; }

        /// <summary>
        /// UTC date and time when the response was generated.
        /// </summary>
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Correlation identifier used for troubleshooting.
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// Pagination metadata.
        /// Only populated for paginated responses.
        /// </summary>
        public PaginationMetadata? Pagination { get; init; }
    
    }
}
