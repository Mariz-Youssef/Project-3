using ClinicManagementSystem.backend.Features.Doctors.DTOs.Leaves;
using FluentValidation;

namespace ClinicManagementSystem.backend.Features.Doctors.Validators
{
    public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequest>
    {
        public CreateLeaveRequestValidator()
        {
            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("Start date must be before or equal to end date.");

            RuleFor(x => x.Reason)
                .MaximumLength(300);
        }
    }
}
