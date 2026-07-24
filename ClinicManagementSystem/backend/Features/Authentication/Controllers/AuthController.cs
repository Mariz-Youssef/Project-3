using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.Authentication.DTOs;
using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.backend.Features.Authentication.Controllers
{
    /// <summary>
    /// Exposes public endpoints for user registration, login,
    /// and refresh-token exchange/revocation.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new patient login identity. Does not create a Patient
        /// profile — the patient must complete their profile after logging in.
        /// </summary>
        /// <param name="request">The registration details.</param>
        /// <param name="cancellationToken">Token used to observe request cancellation.</param>
        /// <response code="200">Registration succeeded.</response>
        /// <response code="400">Validation failed or the email is already in use.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<RegisterResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register(
            [FromBody] RegisterRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterPatientAsync(request, cancellationToken);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Creates a doctor login identity. Restricted to Admins. Does not create
        /// a Doctor profile — the doctor must complete their profile after logging in.
        /// </summary>
        /// <param name="request">The doctor account details.</param>
        /// <param name="cancellationToken">Token used to observe request cancellation.</param>
        /// <response code="200">The doctor account was created.</response>
        /// <response code="400">Validation failed or the email is already in use.</response>
        /// <response code="401">The caller is not authenticated.</response>
        /// <response code="403">The caller is authenticated but is not an Admin.</response>
        [HttpPost("create-doctor-account")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(typeof(ApiResponse<DoctorAccountResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<DoctorAccountResponseDto>>> CreateDoctorAccount(
            [FromBody] CreateDoctorAccountRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await _authService.CreateDoctorAccountAsync(request, cancellationToken);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Authenticates a user with email and password.
        /// </summary>
        /// <param name="request">The login credentials.</param>
        /// <param name="cancellationToken">Token used to observe request cancellation.</param>
        /// <response code="200">Login succeeded.</response>
        /// <response code="401">Invalid credentials or the account is locked out.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(
            [FromBody] LoginRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(request, GetClientIp(), cancellationToken);

            return result.Success ? Ok(result) : Unauthorized(result);
        }

        /// <summary>
        /// Exchanges a valid refresh token for a new access/refresh token pair.
        /// The presented refresh token is revoked as part of rotation.
        /// </summary>
        /// <param name="request">The refresh token to exchange.</param>
        /// <param name="cancellationToken">Token used to observe request cancellation.</param>
        /// <response code="200">The token was refreshed successfully.</response>
        /// <response code="401">The refresh token is invalid, expired, or already used.</response>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken(
            [FromBody] RefreshTokenRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken, GetClientIp(), cancellationToken);

            return result.Success ? Ok(result) : Unauthorized(result);
        }

        /// <summary>
        /// Revokes a refresh token, effectively logging out the device that holds it.
        /// Requires an authenticated caller.
        /// </summary>
        /// <param name="request">The refresh token to revoke.</param>
        /// <param name="cancellationToken">Token used to observe request cancellation.</param>
        /// <response code="200">The token was revoked successfully.</response>
        /// <response code="400">The token is invalid or already revoked.</response>
        /// <response code="401">The caller is not authenticated.</response>
        [HttpPost("revoke-token")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<bool>>> RevokeToken(
            [FromBody] RefreshTokenRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await _authService.RevokeTokenAsync(request.RefreshToken, GetClientIp(), cancellationToken);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        /// <summary>
        /// Retrieves the caller's IP address for auditing refresh token activity.
        /// </summary>
        private string? GetClientIp() => HttpContext.Connection.RemoteIpAddress?.ToString();
    }

}

