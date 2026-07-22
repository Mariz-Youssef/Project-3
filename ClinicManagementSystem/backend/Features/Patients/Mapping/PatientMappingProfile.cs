using AutoMapper;
using ClinicManagementSystem.backend.Enums;
using ClinicManagementSystem.backend.Features.Patients.DTOs;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Patients.Mapping;

public sealed class PatientMappingProfile : Profile
{
    public PatientMappingProfile()
    {
        CreateMap<Patient, PatientResponseDto>()
            .ForMember(destination => destination.FullName, option =>
                option.MapFrom(source => source.User.FullName))
            .ForMember(destination => destination.Email, option =>
                option.MapFrom(source => source.User.Email ?? string.Empty))
            .ForMember(destination => destination.Allergies, option =>
                option.MapFrom(source => source.Allergies ?? string.Empty))
            .ForMember(destination => destination.DateOfBirth, option =>
                option.MapFrom(source => source.DateOfBirth.ToDateTime(TimeOnly.MinValue)))
            .ForMember(destination => destination.Gender, option =>
                option.MapFrom(source => source.Gender.ToString()))
            .ForMember(destination => destination.BloodGroup, option =>
                option.MapFrom(source => ToBloodGroupCode(source.BloodGroup)));

        CreateMap<CreatePatientDto, Patient>()
            .ForMember(destination => destination.UserId, option => option.Ignore())
            .ForMember(destination => destination.DateOfBirth, option =>
                option.MapFrom(source => DateOnly.FromDateTime(source.DateOfBirth)))
            .ForMember(destination => destination.Gender, option =>
                option.MapFrom(source => ParseGender(source.Gender)))
            .ForMember(destination => destination.BloodGroup, option =>
                option.MapFrom(source => ParseBloodGroup(source.BloodGroup)));

        CreateMap<UpdatePatientDto, Patient>()
            .ForMember(destination => destination.UserId, option => option.Ignore())
            .ForMember(destination => destination.DateOfBirth, option =>
            {
                option.PreCondition(source => source.DateOfBirth.HasValue);
                option.MapFrom(source => DateOnly.FromDateTime(source.DateOfBirth!.Value));
            })
            .ForMember(destination => destination.Gender, option =>
                option.MapFrom(source => ParseGender(source.Gender)))
            .ForMember(destination => destination.BloodGroup, option =>
                option.MapFrom(source => ParseBloodGroup(source.BloodGroup)));
    }

    private static Gender ParseGender(string gender)
    {
        return Enum.Parse<Gender>(gender.Trim(), ignoreCase: true);
    }

    private static BloodGroup ParseBloodGroup(string bloodGroup)
    {
        return bloodGroup.Trim().ToUpperInvariant() switch
        {
            "A+" => BloodGroup.APositive,
            "A-" => BloodGroup.ANegative,
            "B+" => BloodGroup.BPositive,
            "B-" => BloodGroup.BNegative,
            "AB+" => BloodGroup.ABPositive,
            "AB-" => BloodGroup.ABNegative,
            "O+" => BloodGroup.OPositive,
            "O-" => BloodGroup.ONegative,
            _ => Enum.Parse<BloodGroup>(bloodGroup.Trim(), ignoreCase: true)
        };
    }

    private static string ToBloodGroupCode(BloodGroup bloodGroup)
    {
        return bloodGroup switch
        {
            BloodGroup.APositive => "A+",
            BloodGroup.ANegative => "A-",
            BloodGroup.BPositive => "B+",
            BloodGroup.BNegative => "B-",
            BloodGroup.ABPositive => "AB+",
            BloodGroup.ABNegative => "AB-",
            BloodGroup.OPositive => "O+",
            BloodGroup.ONegative => "O-",
            _ => bloodGroup.ToString()
        };
    }
}