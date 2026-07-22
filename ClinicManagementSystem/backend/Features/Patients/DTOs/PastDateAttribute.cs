using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Features.Patients.DTOs;

public sealed class PastDateAttribute : ValidationAttribute
{
    public PastDateAttribute()
    {
        ErrorMessage = "Date of birth must be in the past.";
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        return value switch
        {
            DateTime dateTime => dateTime.Date < DateTime.UtcNow.Date,
            _ => false
        };
    }
}