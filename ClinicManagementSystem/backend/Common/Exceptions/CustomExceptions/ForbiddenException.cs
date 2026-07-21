namespace ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Thrown when the current user is not allowed to access the requested resource.
    /// </summary>
    public sealed class ForbiddenException : AppException
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}
