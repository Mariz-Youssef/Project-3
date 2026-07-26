using AutoMapper;
using ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Requests;
using ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Responses;
using ClinicManagementSystem.backend.Models;


namespace ClinicManagementSystem.backend.Features.DepartmentFeature;

/// <summary>
/// AutoMapper profile for department mappings.
/// </summary>
public class DepartmentProfile:Profile
{
    public DepartmentProfile()
    {
        CreateMap<CreateDepartmentRequestDto, Department>();

        CreateMap<UpdateDepartmentRequestDto, Department>();

        CreateMap<Department, DepartmentResponseDto>();

        CreateMap<Department, DepartmentDetailsResponseDto>()
            .ForMember(
                destination => destination.DoctorsCount,
                options => options.MapFrom(source => source.Doctors.Count));
    }
}
