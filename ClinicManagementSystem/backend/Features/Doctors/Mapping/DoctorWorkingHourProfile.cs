using AutoMapper;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.WorkingHours;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Doctors.Mapping;

public class DoctorWorkingHourProfile : Profile
{
    public DoctorWorkingHourProfile()
    {
        CreateMap<CreateWorkingHourRequest, DoctorWorkingHour>();
        CreateMap<UpdateWorkingHourRequest, DoctorWorkingHour>();
        CreateMap<DoctorWorkingHour, WorkingHourResponse>();
    }
}