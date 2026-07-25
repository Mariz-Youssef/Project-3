using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Settings;
using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using ClinicManagementSystem.backend.Features.Authentication.Models;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace ClinicManagementSystem.backend.Features.Authentication.Validators
{
    /// <inheritdoc cref="IGoogleTokenValidator"/>

    public class GoogleTokenValidator : IGoogleTokenValidator
    {
        private readonly GoogleSettings _googleSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleTokenValidator"/> class.
        /// </summary>
        /// <param name="googleSettings">The Google Sign-In configuration options.</param>
        public GoogleTokenValidator(IOptions<GoogleSettings> googleSettings)
        {
            _googleSettings = googleSettings.Value;
        }

        /// <inheritdoc/>
        public async Task<GoogleUserPayload> ValidateAsync(string idToken)
        {
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleSettings.ClientId }
            };

            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(
                    idToken,
                    validationSettings);
            }
            catch (InvalidJwtException)
            {
                throw new UnauthorizedException("Invalid Google token.");
            }

            return new GoogleUserPayload
            {
                Subject = payload.Subject,
                Email = payload.Email,
                FullName = payload.Name
            };
        }
    }
}
