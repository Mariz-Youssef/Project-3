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
        /// Initializes the mapping configuration for the Authentication feature.
        /// </summary>
        public AuthMappingProfile()
        {
            // RegisterRequestDto -> ApplicationUser
            // Password is excluded: UserManager hashes it separately via CreateAsync.
            CreateMap<RegisterRequestDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            // CreateDoctorAccountRequestDto -> ApplicationUser
            // Same shape as registration; kept as a separate map so the two
            // request types can diverge independently without affecting each other.
            CreateMap<CreateDoctorAccountRequestDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            // ApplicationUser -> AuthResponseDto
            // Roles and token fields cannot be derived from ApplicationUser alone;
            // they are explicitly ignored here and set by AuthService afterward.
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