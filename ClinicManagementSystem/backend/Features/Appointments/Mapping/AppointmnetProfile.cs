using AutoMapper;
using ClinicManagementSystem.backend.Features.Appointments.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Appointments.DTOs.Responses;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Appointments.Mapping
{
    /// <summary>
    /// AutoMapper profile for appointment mappings.
    /// </summary>
    public class AppointmnetProfile:Profile
    {
        public AppointmnetProfile()
        {
            #region Request -> Entity

            CreateMap<CreateAppointmentRequestDto, Appointment>()
                .ForMember(
                    destination => destination.Status,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.PatientId,
                    options => options.Ignore());

            CreateMap<UpdateAppointmentRequestDto, Appointment>()
                .ForMember(
                    destination => destination.Id,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.DoctorId,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.PatientId,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.Status,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.CreatedAt,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.UpdatedAt,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.IsDeleted,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.DeletedAt,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.Doctor,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.Patient,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.MedicalRecord,
                    options => options.Ignore());

            #endregion


            #region Entity -> Response

            CreateMap<Appointment, AppointmentResponseDto>()
                .ForMember(
                    destination => destination.DoctorName,
                    options => options.MapFrom(source => source.Doctor.User.FullName))
                .ForMember(
                    destination => destination.PatientName,
                    options => options.MapFrom(source => source.Patient.User.FullName));

            CreateMap<Appointment, AppointmentDetailsResponseDto>()
                .ForMember(
                    destination => destination.DoctorName,
                    options => options.MapFrom(source => source.Doctor.User.FullName))
                .ForMember(
                    destination => destination.PatientName,
                    options => options.MapFrom(source => source.Patient.User.FullName));

            #endregion


        }
    }
}
