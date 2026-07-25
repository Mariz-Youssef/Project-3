using AutoMapper;
using ClinicManagementSystem.backend.Common.Constants;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Features.Authentication.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Authentication.DTOs.Responses;
using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using ClinicManagementSystem.backend.Features.Authentication.Models;
using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementSystem.backend.Features.Authentication.Services
{
    /// <inheritdoc cref="IGoogleAuthenticationService"/>

    public class GoogleAuthenticationService: IGoogleAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGoogleTokenValidator _googleTokenValidator;
        private readonly IAuthTokenIssuer _authTokenIssuer;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAuthenticationService"/> class.
        /// </summary>
        public GoogleAuthenticationService(
            UserManager<ApplicationUser> userManager,
            IGoogleTokenValidator googleTokenValidator,
            IAuthTokenIssuer authTokenIssuer,
            IMapper mapper)
        {
            _userManager = userManager;
            _googleTokenValidator = googleTokenValidator;
            _authTokenIssuer = authTokenIssuer;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<AuthResponseDto> LoginAsync(
            GoogleLoginRequestDto request,
            string? ipAddress,
            CancellationToken cancellationToken = default)
        {
            var payload = await _googleTokenValidator.ValidateAsync(request.IdToken);

            var user = await _userManager.FindByLoginAsync(
                AuthConstants.GoogleProvider,
                payload.Subject);

            user ??= await FindOrCreateUserAsync(payload,cancellationToken);

            if (user.IsDeleted)
            {
                throw new UnauthorizedException("This account is no longer active.");
            }

            return await _authTokenIssuer.IssueTokensAsync(user, ipAddress, cancellationToken);
        }

        /// <summary>
        /// Links a Google login to an existing account matched by email,
        /// or creates a new patient account if none exists.
        /// </summary>
        private async Task<ApplicationUser> FindOrCreateUserAsync(GoogleUserPayload payload, CancellationToken cancellationToken= default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var existingUser = await _userManager.FindByEmailAsync(payload.Email);

            if (existingUser is not null)
            {
                await LinkGoogleLoginAsync(existingUser, payload);
                return existingUser;
            }

            var newUser = _mapper.Map<ApplicationUser>(payload);

            var createResult = await _userManager.CreateAsync(newUser);

            if (!createResult.Succeeded)
            {
                throw new ValidationException(
                    createResult.Errors.Select(e => e.Description));
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, RoleNames.Patient);

            if (!roleResult.Succeeded)
            {
                throw new ValidationException(
                    roleResult.Errors.Select(e => e.Description));
            }

            await LinkGoogleLoginAsync(newUser, payload , cancellationToken);

            return newUser;
        }

        /// <summary>
        /// Associates a Google identity with the given application user via
        /// ASP.NET Identity's external login store.
        /// </summary>
        private async Task LinkGoogleLoginAsync(
            ApplicationUser user,
            GoogleUserPayload payload,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var linkedUser = await _userManager.FindByLoginAsync(
                AuthConstants.GoogleProvider,
                payload.Subject);

            if (linkedUser is not null)
            {
                if (linkedUser.Id != user.Id)
                {
                    throw new ConflictException(
                        "This Google account is already linked to another user.");
                }

                return;
            }

            var loginInfo = new UserLoginInfo(
                AuthConstants.GoogleProvider,
                payload.Subject,
                AuthConstants.GoogleProvider);

            var result = await _userManager.AddLoginAsync(
                user,
                loginInfo);

            if (!result.Succeeded)
            {
                throw new ValidationException(
                    result.Errors.Select(e => e.Description));
            }
        }
    }
}
