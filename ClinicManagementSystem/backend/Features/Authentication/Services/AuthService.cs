using AutoMapper;
using ClinicManagementSystem.backend.Common.Data;
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
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Roles that a user may request through public self-registration.
        /// Any other role (Admin, Doctor, Receptionist) must be assigned
        /// by an administrator through a separate, authorized endpoint.
        /// </summary>
        private static readonly string[] AllowedSelfRegisterRoles = { "Patient" };

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        public AuthService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ITokenService tokenService,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        /// <inheritdoc />
        public async Task<AuthServiceResult<RegisterResponseDto>> RegisterAsync(
    RegisterRequestDto request,
    CancellationToken cancellationToken = default)
        {
            if (!AllowedSelfRegisterRoles.Contains(request.Role))
            {
                return AuthServiceResult<RegisterResponseDto>.Fail(
                    $"Self-registration is only allowed for the following roles: {string.Join(", ", AllowedSelfRegisterRoles)}.");
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return AuthServiceResult<RegisterResponseDto>.Fail("A user with this email already exists.");
            }

            var user = _mapper.Map<ApplicationUser>(request);

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return AuthServiceResult<RegisterResponseDto>.Fail(
                    "Registration failed.",
                    createResult.Errors.Select(e => e.Description).ToList());
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            var response = new RegisterResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = request.Role
            };

            return AuthServiceResult<RegisterResponseDto>.Ok(response, "Registration successful. Please log in.");
        }

        /// <inheritdoc />
        public async Task<AuthServiceResult<AuthResponseDto>> LoginAsync(
            LoginRequestDto request,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || user.IsDeleted)
            {
                return AuthServiceResult<AuthResponseDto>.Fail("Invalid email or password.");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return AuthServiceResult<AuthResponseDto>.Fail("Account is locked. Please try again later.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                await _userManager.AccessFailedAsync(user);
                return AuthServiceResult<AuthResponseDto>.Fail("Invalid email or password.");
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            var authResponse = await GenerateAuthResponseAsync(user, ipAddress, cancellationToken);
            return AuthServiceResult<AuthResponseDto>.Ok(authResponse, "Login successful.");
        }

        /// <inheritdoc />
        public async Task<AuthServiceResult<AuthResponseDto>> RefreshTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

            if (storedToken is null || !storedToken.IsActive)
            {
                return AuthServiceResult<AuthResponseDto>.Fail("Invalid or expired refresh token.");
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

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            var roles = await _userManager.GetRolesAsync(storedToken.User);
            var (accessToken, accessTokenExpiresAt) = _tokenService.GenerateAccessToken(storedToken.User, roles);

            var response = _mapper.Map<AuthResponseDto>(storedToken.User);
            response.Roles = roles;
            response.AccessToken = accessToken;
            response.AccessTokenExpiresAt = accessTokenExpiresAt;
            response.RefreshToken = newRefreshTokenValue;
            response.RefreshTokenExpiresAt = newRefreshToken.ExpiresAt;

            return AuthServiceResult<AuthResponseDto>.Ok(response, "Token refreshed successfully.");
        }

        /// <inheritdoc />
        public async Task<AuthServiceResult<bool>> RevokeTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

            if (storedToken is null || !storedToken.IsActive)
            {
                return AuthServiceResult<bool>.Fail("Invalid or already revoked token.");
            }

            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = ipAddress;

            await _context.SaveChangesAsync(cancellationToken);

            return AuthServiceResult<bool>.Ok(true, "Token revoked successfully.");
        }

        /// <summary>
        /// Generates a new access/refresh token pair for the given user,
        /// persists the refresh token, and maps the result to <see cref="AuthResponseDto"/>.
        /// </summary>
        /// <param name="user">The user to issue tokens for.</param>
        /// <param name="ipAddress">The client IP address to record against the refresh token.</param>
        /// <param name="cancellationToken">Token used to observe cancellation requests.</param>
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

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

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
