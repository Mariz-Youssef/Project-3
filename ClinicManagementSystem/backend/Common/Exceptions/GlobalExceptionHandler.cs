using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Responses;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ClinicManagementSystem.backend.Common.Exceptions
{
    /// <summary>
    /// Handles all unhandled exceptions globally.
    /// </summary>
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            LogException(exception);
            HttpStatusCode statusCode = GetStatusCode(exception);

            ApiResponse<object> response = exception switch
            {
                ValidationException validationException =>
                    ApiResponseFactory.ValidationFailure(validationException.Errors),

                AppException =>
                    ApiResponseFactory.Failure<object>(exception.Message),

                _ =>
                    ApiResponseFactory.Failure<object>(
                        "An unexpected error occurred. Please try again later.")
            };

            // Add Trace Identifier
            response.TraceId = httpContext.TraceIdentifier;


            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(
                response,
                cancellationToken: cancellationToken);

            return true;

        }


        /// <summary>
        /// Logs the exception using the appropriate log level.
        /// </summary>
        private void LogException(Exception exception)
        {
            if (exception is AppException)
            {
                _logger.LogWarning(exception, exception.Message);
            }
            else
            {
                _logger.LogError(exception, "An unexpected exception occurred.");

            }
        }


        /// <summary>
        /// Maps exceptions to HTTP status codes.
        /// </summary>
        private static HttpStatusCode GetStatusCode(Exception exception)
        {
            return exception switch
            {
                ValidationException => HttpStatusCode.BadRequest,

                BadRequestException => HttpStatusCode.BadRequest,

                UnauthorizedException => HttpStatusCode.Unauthorized,

                ForbiddenException => HttpStatusCode.Forbidden,

                NotFoundException => HttpStatusCode.NotFound,

                ConflictException => HttpStatusCode.Conflict,

                _ => HttpStatusCode.InternalServerError
            };
        }

    }
}
