using ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests;
using FluentValidation;

namespace ClinicManagementSystem.backend.Features.Doctors.Validators
{
    public class CreateDoctorValidator : AbstractValidator<CreateDoctorRequest>
    {
        public CreateDoctorValidator()
        {
            RuleFor(x => x.UserId)
            .GreaterThan(0);

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
