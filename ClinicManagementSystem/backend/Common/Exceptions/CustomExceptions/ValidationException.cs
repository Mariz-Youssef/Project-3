namespace ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Thrown when one or more validation errors occur.
    /// </summary>
    public sealed class ValidationException : AppException
    {
        public IReadOnlyList<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors.ToList().AsReadOnly();
        }

        public ValidationException(string error)
            : this(new[] { error })
        {
        }

    }
}
