using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Pagination;


namespace ClinicManagementSystem.backend.Common.Responses
{
    /// <summary>
    /// Creates standardized API responses.
    /// </summary>
    public static class ApiResponseFactory
    {
        #region Success Responses

        /// <summary>
        /// Creates a successful response.
        /// </summary>
        public static ApiResponse<T> Success<T>(T data, string resourceName, ResponseAction action)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = BuildMessage(resourceName, action),
                Data = data
            };
        }

        /// <summary>
        /// Creates a successful paginated response.
        /// </summary>
        public static ApiResponse<T> Success<T>(T data, PaginationMetadata pagination, string resourceName, ResponseAction action)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = BuildMessage(resourceName, action),
                Data = data,
                Pagination = pagination
            };
        }

        #endregion

        #region Failure Responses

        /// <summary>
        /// Creates a failed response.
        /// </summary>
        public static ApiResponse<T> Failure<T>(string message, IEnumerable<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors?.ToList()
            };
        }

        /// <summary>
        /// Creates a validation failure response.
        /// </summary>
        public static ApiResponse<object> ValidationFailure(IEnumerable<string> errors)
        {
            return Failure<object>(ResponseMessages.ValidationFailed, errors);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Builds a standardized response message
        /// based on the resource name and action.
        /// </summary>
        private static string BuildMessage(string resourceName, ResponseAction action)
        {
            return action switch
            {
                ResponseAction.Created =>
                    ResponseMessageBuilder.Created(resourceName),

                ResponseAction.Updated =>
                    ResponseMessageBuilder.Updated(resourceName),

                ResponseAction.Deleted =>
                    ResponseMessageBuilder.Deleted(resourceName),

                ResponseAction.Retrieved =>
                    ResponseMessageBuilder.Retrieved(resourceName),

                ResponseAction.RetrievedList =>
                    ResponseMessageBuilder.RetrievedList(resourceName),

                _ => throw new ArgumentOutOfRangeException(nameof(action), action, "Unsupported response action.")

            };
        }

        #endregion
    }
}