using AutoMapper;
using ClinicManagementSystem.backend.Features.Authentication.DTOs;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Authentication.Mapping
{
    /// <summary>
    /// Defines AutoMapper mappings between Auth-related entities and DTOs.
    /// </summary>
    public class AuthMappingProfile : Profile
    {
        /// <summary>
        /// Initializes the mapping configuration for the Auth feature.
        /// </summary>
        public AuthMappingProfile()
        {
            // RegisterRequestDto -> ApplicationUser
            // Password is intentionally excluded: UserManager hashes it separately.
            CreateMap<RegisterRequestDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            // ApplicationUser -> AuthResponseDto
            // Roles and token fields cannot be derived from ApplicationUser alone,
            // so they are explicitly ignored here and populated by the service
            // after the base mapping is applied.
            CreateMap<ApplicationUser, AuthResponseDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.AccessTokenExpiresAt, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokenExpiresAt, opt => opt.Ignore());
        }
    }
}