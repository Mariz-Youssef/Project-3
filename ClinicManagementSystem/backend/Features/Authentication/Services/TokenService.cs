using ClinicManagementSystem.backend.Common.Settings;
using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using ClinicManagementSystem.backend.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ClinicManagementSystem.backend.Features.Authentication.Services
{
    /// <inheritdoc cref="ITokenService"/>

    public class TokenService: ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="jwtSettings">The JWT configuration options.</param>

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public (string Token, DateTime ExpiresAt) GenerateAccessToken(
            ApplicationUser user,
            IList<string> roles)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(
                _jwtSettings.AccessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: BuildClaims(user, roles),
                expires: expiresAt,
                signingCredentials: CreateSigningCredentials());

            return (_tokenHandler.WriteToken(token), expiresAt);
        }

        public string GenerateRefreshTokenValue()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        /// <summary>
        /// Builds the claims included in the JWT access token.
        /// </summary>
        private static IEnumerable<Claim> BuildClaims(
            ApplicationUser user,
            IEnumerable<string> roles)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            claims.AddRange(
                roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        /// <summary>
        /// Creates the signing credentials used to sign JWT access tokens.
        /// </summary>
        private SigningCredentials CreateSigningCredentials()
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            return new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
        }
    }
}
