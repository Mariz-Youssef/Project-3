using ClinicManagementSystem.backend.Features.Doctors.DTOs.WorkingHours;
using FluentValidation;

namespace ClinicManagementSystem.backend.Features.Doctors.Validators
{
    public class UpdateWorkingHourRequestValidator : AbstractValidator<UpdateWorkingHourRequest>
    {
        public UpdateWorkingHourRequestValidator()
        {
            RuleFor(x => x.DayOfWeek)
                .IsInEnum()
                .WithMessage("Invalid day of week.");

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime)
                .WithMessage("Start time must be earlier than end time.");
        }
    }
}
