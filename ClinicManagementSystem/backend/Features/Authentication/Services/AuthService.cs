using AutoMapper;
using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Data;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Common.Settings;
using ClinicManagementSystem.backend.Features.Authentication.DTOs;
using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClinicManagementSystem.backend.Features.Authentication.Services
{
    /// <inheritdoc cref="IAuthService"/>

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        public AuthService(
            UserManager<ApplicationUser> userManager,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        /// <inheritdoc />
        public async Task<ApiResponse<RegisterResponseDto>> RegisterPatientAsync(
            RegisterRequestDto request,
            CancellationToken cancellationToken = default)
        {
            // Note: UserManager does not expose CancellationToken overloads.
            // This is a limitation of ASP.NET Core Identity's public API.
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return ApiResponse<RegisterResponseDto>.FailureResponse("A user with this email already exists.");
            }

            var user = _mapper.Map<ApplicationUser>(request);

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return ApiResponse<RegisterResponseDto>.FailureResponse(
                    "Registration failed.",
                    createResult.Errors.Select(e => e.Description).ToList());
            }

            await _userManager.AddToRoleAsync(user, RoleNames.Patient);

            var response = new RegisterResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = RoleNames.Patient
            };

            return ApiResponse<RegisterResponseDto>.SuccessResponse(
                response,
                "Registration successful. Please log in and complete your patient profile.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<DoctorAccountResponseDto>> CreateDoctorAccountAsync(
            CreateDoctorAccountRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return ApiResponse<DoctorAccountResponseDto>.FailureResponse("A user with this email already exists.");
            }

            var user = _mapper.Map<ApplicationUser>(request);

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return ApiResponse<DoctorAccountResponseDto>.FailureResponse(
                    "Doctor account creation failed.",
                    createResult.Errors.Select(e => e.Description).ToList());
            }

            await _userManager.AddToRoleAsync(user, RoleNames.Doctor);

            var response = new DoctorAccountResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty
            };

            return ApiResponse<DoctorAccountResponseDto>.SuccessResponse(
                response,
                "Doctor account created. The doctor must log in and complete their profile.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(
            LoginRequestDto request,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || user.IsDeleted)
            {
                return ApiResponse<AuthResponseDto>.FailureResponse("Invalid email or password.");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return ApiResponse<AuthResponseDto>.FailureResponse("Account is locked. Please try again later.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                await _userManager.AccessFailedAsync(user);
                return ApiResponse<AuthResponseDto>.FailureResponse("Invalid email or password.");
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            var authResponse = await GenerateAuthResponseAsync(user, ipAddress, cancellationToken);
            return ApiResponse<AuthResponseDto>.SuccessResponse(authResponse, "Login successful.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

            if (storedToken is null || !storedToken.IsActive)
            {
                return ApiResponse<AuthResponseDto>.FailureResponse("Invalid or expired refresh token.");
            }

            // Rotate: revoke the presented token and chain it to its replacement.
            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = ipAddress;

            var newRefreshTokenValue = _tokenService.GenerateRefreshTokenValue();
            storedToken.ReplacedByToken = newRefreshTokenValue;

            var newRefreshToken = new RefreshToken
            {
                Token = newRefreshTokenValue,
                UserId = storedToken.UserId,
                CreatedByIp = ipAddress,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            var roles = await _userManager.GetRolesAsync(storedToken.User);
            var (accessToken, accessTokenExpiresAt) = _tokenService.GenerateAccessToken(storedToken.User, roles);

            var response = _mapper.Map<AuthResponseDto>(storedToken.User);
            response.Roles = roles;
            response.AccessToken = accessToken;
            response.AccessTokenExpiresAt = accessTokenExpiresAt;
            response.RefreshToken = newRefreshTokenValue;
            response.RefreshTokenExpiresAt = newRefreshToken.ExpiresAt;

            return ApiResponse<AuthResponseDto>.SuccessResponse(response, "Token refreshed successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> RevokeTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

            if (storedToken is null || !storedToken.IsActive)
            {
                return ApiResponse<bool>.FailureResponse("Invalid or already revoked token.");
            }

            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = ipAddress;

            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResponse(true, "Token revoked successfully.");
        }

        /// <summary>
        /// Generates a new access/refresh token pair for the given user,
        /// persists the refresh token, and maps the result to <see cref="AuthResponseDto"/>.
        /// </summary>
        private async Task<AuthResponseDto> GenerateAuthResponseAsync(
            ApplicationUser user,
            string? ipAddress,
            CancellationToken cancellationToken)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var (accessToken, accessTokenExpiresAt) = _tokenService.GenerateAccessToken(user, roles);
            var refreshTokenValue = _tokenService.GenerateRefreshTokenValue();

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenValue,
                UserId = user.Id,
                CreatedByIp = ipAddress,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<AuthResponseDto>(user);
            response.Roles = roles;
            response.AccessToken = accessToken;
            response.AccessTokenExpiresAt = accessTokenExpiresAt;
            response.RefreshToken = refreshTokenValue;
            response.RefreshTokenExpiresAt = refreshToken.ExpiresAt;

            return response;
        }
    }
}
