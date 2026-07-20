namespace ClinicManagementSystem.backend.Common.Responses
{
    /// <summary>
    /// Generic wrapper providing a consistent success/error response shape
    /// across all API endpoints.
    /// </summary>
    /// <typeparam name="T">The type of the response payload.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>Gets or sets a value indicating whether the operation succeeded.</summary>
        public bool Success { get; set; }

        /// <summary>Gets or sets a human-readable message describing the result.</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>Gets or sets the response payload, if any.</summary>
        public T? Data { get; set; }

        /// <summary>Gets or sets a list of error details, if any.</summary>
        public IList<string>? Errors { get; set; }
        /// <summary>Creates a successful response.</summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success") =>
            new() { Success = true, Message = message, Data = data };

        /// <summary>Creates a failed response.</summary>
        public static ApiResponse<T> FailureResponse(string message, IList<string>? errors = null) =>
            new() { Success = false, Message = message, Errors = errors };
    }
}
