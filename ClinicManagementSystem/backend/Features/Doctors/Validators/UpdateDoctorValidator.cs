using ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests;
using FluentValidation;

namespace ClinicManagementSystem.backend.Features.Doctors.Validators
{
    public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorRequest>
    {
        public UpdateDoctorValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0);

            RuleFor(x => x.Specialization)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LicenseNumber)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.YearsOfExperience)
                .InclusiveBetween(0, 60);

            RuleFor(x => x.ConsultationFee)
                .InclusiveBetween(0, 100000);
        }

    }
}
