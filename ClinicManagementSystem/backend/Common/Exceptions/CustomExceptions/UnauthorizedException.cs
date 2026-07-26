namespace ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Thrown when authentication is required.
    /// </summary>
    public sealed class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}
