using AutoMapper;
using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Data;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Responses;
using ClinicManagementSystem.backend.Common.Settings;
using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.Authentication.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Authentication.DTOs.Responses;
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
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IAuthTokenIssuer _authTokenIssuer;


        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        public AuthService(
            UserManager<ApplicationUser> userManager,
            IRefreshTokenRepository refreshTokenRepository,

            IMapper mapper,
            ApplicationDbContext context,
            IAuthTokenIssuer authTokenIssuer)
        {
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
            _context = context;
            _authTokenIssuer = authTokenIssuer;
        }

       
        /// <inheritdoc/>
        public async Task<RegisterResponseDto> RegisterPatientAsync(
            RegisterRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<ApplicationUser>(request);

            await CreateUserAsync(
                user,
                request.Password,
                RoleNames.Patient);

            var patient = new Patient
            {
                UserId = user.Id
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync(cancellationToken);
            
            return new RegisterResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = RoleNames.Patient
            };
        }
        /// <inheritdoc/>
        public async Task<DoctorAccountResponseDto> CreateDoctorAccountAsync(
            CreateDoctorAccountRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<ApplicationUser>(request);

            await CreateUserAsync(
                user,
                request.Password,
                RoleNames.Doctor);
            var doctor = new Doctor
            {
                UserId = user.Id,
        
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync(cancellationToken);

            return new DoctorAccountResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty
            };
        }
        /// <inheritdoc/>
        public async Task<AuthResponseDto> LoginAsync(
            LoginRequestDto request,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || user.IsDeleted)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                throw new UnauthorizedException(
                    "Your account is locked. Please try again later.");
            }

            var passwordValid =
                await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
            {
                await _userManager.AccessFailedAsync(user);

                throw new UnauthorizedException("Invalid email or password.");
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            return await _authTokenIssuer.IssueTokensAsync(
                 user,
                 ipAddress,
                 cancellationToken);
        }
        /// <inheritdoc />
        public async Task<AuthResponseDto> RefreshTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var storedToken =
                await _refreshTokenRepository.GetByTokenAsync(
                    refreshToken,
                    cancellationToken);

            if (storedToken is null)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            if (storedToken.RevokedAt is not null)
            {
                await _refreshTokenRepository.RevokeDescendantsAsync(
                    storedToken,
                    ipAddress,
                    cancellationToken);

                await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

                throw new UnauthorizedException(
                    "This refresh token has already been used. All sessions have been revoked for security.");
            }

            if (storedToken.IsExpired)
            {
                throw new UnauthorizedException("Refresh token has expired.");
            }

            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = ipAddress;

            var response = await _authTokenIssuer.IssueTokensAsync(
                storedToken.User,
                ipAddress,
                cancellationToken);

            storedToken.ReplacedByToken = response.RefreshToken;

            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return response;
        }
        /// <inheritdoc/>
        public async Task RevokeTokenAsync(
            string refreshToken,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var storedToken = await _refreshTokenRepository
                .GetByTokenAsync(refreshToken, cancellationToken);

            if (storedToken is null || !storedToken.IsActive)
            {
                throw new UnauthorizedException(
                    "Invalid or already revoked refresh token.");
            }

            storedToken.RevokedAt = DateTime.UtcNow;
            storedToken.RevokedByIp = ipAddress;

            await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        }


        /// <inheritdoc/>
        public async Task ChangePasswordAsync(
            int userId,
            ChangePasswordRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null || user.IsDeleted)
            {
                throw new UnauthorizedException("Invalid user.");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
            {
                throw new ValidationException(
                    result.Errors.Select(e => e.Description));
            }
            await _refreshTokenRepository.RevokeAllForUserAsync(userId, cancellationToken);
        }

        /// <summary>
        /// Creates a new Identity user and assigns the specified role.
        /// </summary>
        private async Task CreateUserAsync(
     ApplicationUser user,
     string password,
     string role,
     CancellationToken cancellationToken=default)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email!);

            if (existingUser is not null)
            {
                throw new ConflictException(
                    ResponseMessageBuilder.AlreadyExists("User"));
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var createResult = await _userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                throw new ValidationException(
                    createResult.Errors.Select(e => e.Description));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                throw new ValidationException(
                    roleResult.Errors.Select(e => e.Description));
            }

            await transaction.CommitAsync(cancellationToken);
        }


    }
}
