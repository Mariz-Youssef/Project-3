using AutoMapper;
using ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Prescriptions.DTOs.Responses;
using ClinicManagementSystem.backend.Models;

namespace ClinicManagementSystem.backend.Features.Prescriptions.Mapping
{
    /// <summary>
    /// Defines AutoMapper mappings for the Prescription feature.
    /// </summary>
    public class PrescriptionProfile:Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrescriptionProfile"/> class.
        /// </summary>
        public PrescriptionProfile()
        {
            // ============================================================
            // Entity -> Response DTO
            // ============================================================

            CreateMap<Prescription, PrescriptionResponseDto>()

                // Medical Record
                .ForMember(
                    destination => destination.MedicalRecordId,
                    options => options.MapFrom(source =>
                        source.MedicalRecordId))

                // Appointment
                .ForMember(
                    destination => destination.AppointmentId,
                    options => options.MapFrom(source =>
                        source.MedicalRecord.AppointmentId))

                // Doctor
                .ForMember(
                    destination => destination.DoctorId,
                    options => options.MapFrom(source =>
                        source.MedicalRecord.Appointment.DoctorId))

                .ForMember(
                    destination => destination.DoctorName,
                    options => options.MapFrom(source =>
                        source.MedicalRecord.Appointment.Doctor.User.FullName))

                // Patient
                .ForMember(
                    destination => destination.PatientId,
                    options => options.MapFrom(source =>
                        source.MedicalRecord.Appointment.PatientId))

                .ForMember(
                    destination => destination.PatientName,
                    options => options.MapFrom(source =>
                        source.MedicalRecord.Appointment.Patient.User.FullName));

            // ============================================================
            // Create DTO -> Entity
            // ============================================================

            CreateMap<CreatePrescriptionRequestDto, Prescription>();

            // ============================================================
            // Update DTO -> Existing Entity
            // ============================================================

            CreateMap<UpdatePrescriptionRequestDto, Prescription>()

                // Cannot move a prescription to another Medical Record.
                .ForMember(
                    destination => destination.MedicalRecordId,
                    options => options.Ignore())

                // Managed by EF Core.
                .ForMember(
                    destination => destination.MedicalRecord,
                    options => options.Ignore());
        }
    }
}
