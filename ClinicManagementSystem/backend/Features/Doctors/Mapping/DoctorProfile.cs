using AutoMapper;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Responses;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Doctors.Mapping
{
    public class DoctorProfile: Profile
    {
        public DoctorProfile()
        {
            CreateMap<CreateDoctorRequest, Doctor>();
            CreateMap<UpdateDoctorRequest, Doctor>();

            CreateMap<Doctor, DoctorResponse>()
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.User.FullName))
                .ForMember(d => d.Email,o => o.MapFrom(s => s.User.Email))
                .ForMember(d => d.Department,o => o.MapFrom(s => s.Department.Name));
        }
    }
}
