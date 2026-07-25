using ClinicManagementSystem.backend.Common.Responses;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ClinicManagementSystem.backend.Middleware
{
    /// <summary>
    /// Provides extension methods for configuring rate limiting.
    /// </summary>
    public static class RateLimitingMiddleware
    {
        /// <summary>
        /// Registers application rate limiting.
        /// </summary>
        public static IServiceCollection AddApplicationRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                // Apply rate limiting per client IP.
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    string clientIp =
                        httpContext.Connection.RemoteIpAddress?.ToString()
                        ?? "UnknownClient";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: clientIp,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 30,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            AutoReplenishment = true
                        });
                });

                options.OnRejected = async (context, cancellationToken) =>
                {
                    var response = ApiResponseFactory.Failure<object>(
                        "Too many requests. Please try again later.",
                        new[]
                        {
                            "Rate limit exceeded."
                        });

                    response.TraceId = context.HttpContext.TraceIdentifier;

                    context.HttpContext.Response.ContentType = "application/json";

                    await context.HttpContext.Response.WriteAsJsonAsync(
                        response,
                        cancellationToken: cancellationToken);
                };
            });

            return services;
        }
    }
}