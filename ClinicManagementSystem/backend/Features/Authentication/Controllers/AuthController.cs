using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Extensions;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Features.Authentication.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Authentication.DTOs.Responses;
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
        private readonly IGoogleAuthenticationService _googleAuthenticationService;



        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        /// <param name="googleAuthenticationService">The Google Sign-In authentication service.</param>
        public AuthController(IAuthService authService, IGoogleAuthenticationService googleAuthenticationService)
        {
            _authService = authService;
            _googleAuthenticationService = googleAuthenticationService;
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
     RegisterRequestDto request,
     CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterPatientAsync(
                request,
                cancellationToken);

            return Ok(
                ApiResponseFactory.Success(
                    result,
                    "Patient account",
                    ResponseAction.Created));
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
        public async Task<ActionResult<ApiResponse<DoctorAccountResponseDto>>> CreateDoctorAccount(
     CreateDoctorAccountRequestDto request,
     CancellationToken cancellationToken)
        {
            var result =
                await _authService.CreateDoctorAccountAsync(
                    request,
                    cancellationToken);

            return Ok(
                ApiResponseFactory.Success(
                    result,
                    "Doctor account",
                    ResponseAction.Created));
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
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(
     LoginRequestDto request,
     CancellationToken cancellationToken)
        {
            var result =
                await _authService.LoginAsync(
                    request,
                    GetClientIp(),
                    cancellationToken);

            return Ok(
                ApiResponseFactory.Success(
                    result,
                    "Authentication",
                    ResponseAction.Retrieved));
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
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken(
    RefreshTokenRequestDto request,
    CancellationToken cancellationToken)
        {
            var result =
                await _authService.RefreshTokenAsync(
                    request.RefreshToken,
                    GetClientIp(),
                    cancellationToken);

            return Ok(
                ApiResponseFactory.Success(
                    result,
                    "Authentication",
                    ResponseAction.Retrieved));
        }
        /// <summary>
        /// Revokes a refresh token, effectively logging out the device that holds it.
        /// Requires an authenticated caller.
        /// </summary>
        /// <param name="request">The refresh token to revoke.</param>
        /// <param name="cancellationToken">Token used to observe request cancellation.</param>
        /// <response code="200">The token was revoked successfully.</response>
        /// <response code="401">The token is invalid or already revoked.</response>
        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> RevokeToken(
     RefreshTokenRequestDto request,
     CancellationToken cancellationToken)
        {
            await _authService.RevokeTokenAsync(
                request.RefreshToken,
                GetClientIp(),
                cancellationToken);


            return Ok(
                ApiResponseFactory.Success(
                    "Refresh token",
                    ResponseAction.Deleted));
        }


        /// <summary>
        /// Changes the authenticated user's password. Requires the current
        /// password for verification.
        /// </summary>
        /// <param name="request">The current and new password details.</param>
        /// <param name="cancellationToken">Token used to observe request cancellation.</param>
        /// <response code="200">The password was changed successfully.</response>
        /// <response code="400">Validation failed or the current password is incorrect.</response>
        /// <response code="401">The caller is not authenticated.</response>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword(
            ChangePasswordRequestDto request,
            CancellationToken cancellationToken)
        {
            await _authService.ChangePasswordAsync(
                User.GetUserId(),
                request,
                cancellationToken);

            return Ok(
                ApiResponseFactory.Success(
                    "Password",
                    ResponseAction.Updated));
        }

        /// <summary>
        /// Authenticates a user via Google Sign-In. Automatically creates a
        /// patient account on first sign-in if no matching account exists.
        /// </summary>
        /// <param name="request">The Google ID token to validate.</param>
        /// <param name="cancellationToken">Token used to observe request cancellation.</param>
        /// <response code="200">Login succeeded.</response>
        /// <response code="401">The Google token is invalid or expired.</response>
        [HttpPost("google")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> GoogleLogin(
            GoogleLoginRequestDto request,
            CancellationToken cancellationToken)
        {
            var result = await _googleAuthenticationService.LoginAsync(
                request,
                GetClientIp(),
                cancellationToken);

            return Ok(
                ApiResponseFactory.Success(
                    result,
                    "Authentication",
                    ResponseAction.Retrieved));
        }

        /// <summary>
        /// Retrieves the caller's IP address for auditing refresh token activity.
        /// </summary>
        private string? GetClientIp() => HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}
            
        
        
 

