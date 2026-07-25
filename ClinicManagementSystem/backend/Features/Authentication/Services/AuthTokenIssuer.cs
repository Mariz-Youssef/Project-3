using AutoMapper;
using ClinicManagementSystem.backend.Common.Settings;
using ClinicManagementSystem.backend.Features.Authentication.DTOs.Responses;
using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ClinicManagementSystem.backend.Features.Authentication.Services
{
    /// <inheritdoc cref="IAuthTokenIssuer"/>
    public class AuthTokenIssuer : IAuthTokenIssuer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthTokenIssuer"/> class.
        /// </summary>
        public AuthTokenIssuer(
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

        /// <inheritdoc/>
        public async Task<AuthResponseDto> IssueTokensAsync(
            ApplicationUser user,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var refreshToken = await CreateRefreshTokenAsync(user.Id, ipAddress, cancellationToken);

            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return await BuildAuthResponseAsync(user, refreshToken);
        }

        private async Task<RefreshToken> CreateRefreshTokenAsync(
            int userId,
            string? ipAddress,
            CancellationToken cancellationToken)
        {
            var refreshToken = new RefreshToken
            {
                Token = _tokenService.GenerateRefreshTokenValue(),
                UserId = userId,
                CreatedByIp = ipAddress,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            return refreshToken;
        }

        private async Task<AuthResponseDto> BuildAuthResponseAsync(
            ApplicationUser user,
            RefreshToken refreshToken)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var (accessToken, expiresAt) = _tokenService.GenerateAccessToken(user, roles);

            var response = _mapper.Map<AuthResponseDto>(user);

            response.Roles = roles;
            response.AccessToken = accessToken;
            response.AccessTokenExpiresAt = expiresAt;
            response.RefreshToken = refreshToken.Token;
            response.RefreshTokenExpiresAt = refreshToken.ExpiresAt;

            return response;
        }
    }
}
