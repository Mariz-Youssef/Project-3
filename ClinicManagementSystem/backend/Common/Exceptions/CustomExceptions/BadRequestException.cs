namespace ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Thrown when the client sends an invalid request.
    /// </summary>
    public sealed class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
