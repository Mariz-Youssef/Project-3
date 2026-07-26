using AutoMapper;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Leaves;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Doctors.Mapping
{
    public class DoctorLeaveProfile : Profile
    {
        public DoctorLeaveProfile()
        {
            CreateMap<CreateLeaveRequest, DoctorLeave>();
            CreateMap<UpdateLeaveRequest, DoctorLeave>();
            CreateMap<DoctorLeave, LeaveResponse>();
        }
    }
}
