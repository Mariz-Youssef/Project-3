using AutoMapper;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Requests;
using ClinicManagementSystem.backend.Features.MedicalRecords.DTOs.Responses;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.MedicalRecords.Mapping
{
    /// <summary>
    /// Defines AutoMapper mappings for the Medical Record feature.
    /// </summary>
    public class MedicalRecordProfile:Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MedicalRecordProfile"/> class.
        /// </summary>
        public MedicalRecordProfile()
        {
            // ============================================================
            // Entity -> Response DTO
            // ============================================================

            CreateMap<MedicalRecord, MedicalRecordResponseDto>()

                // Doctor information
                .ForMember(
                    destination => destination.DoctorId,
                    options => options.MapFrom(source =>
                        source.Appointment.DoctorId))

                .ForMember(
                    destination => destination.DoctorName,
                    options => options.MapFrom(source =>
                        source.Appointment.Doctor.User.FullName))

                // Patient information
                .ForMember(
                    destination => destination.PatientId,
                    options => options.MapFrom(source =>
                        source.Appointment.PatientId))

                .ForMember(
                    destination => destination.PatientName,
                    options => options.MapFrom(source =>
                        source.Appointment.Patient.User.FullName))

                // Appointment information
                .ForMember(
                    destination => destination.AppointmentDate,
                    options => options.MapFrom(source =>
                        source.Appointment.AppointmentDate));

            // ============================================================
            // Create DTO -> Entity
            // ============================================================

            CreateMap<CreateMedicalRecordRequestDto, MedicalRecord>();

            // ============================================================
            // Update DTO -> Existing Entity
            // ============================================================

            CreateMap<UpdateMedicalRecordRequestDto, MedicalRecord>()

                // Appointment cannot be changed after record creation.
                .ForMember(
                    destination => destination.AppointmentId,
                    options => options.Ignore())

                // Navigation property is managed by EF Core.
                .ForMember(
                    destination => destination.Appointment,
                    options => options.Ignore())

                // Prescriptions are managed separately.
                .ForMember(
                    destination => destination.Prescriptions,
                    options => options.Ignore());
        }
    }
}
