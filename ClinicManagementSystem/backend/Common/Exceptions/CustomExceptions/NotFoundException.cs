namespace ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Thrown when a requested resource cannot be found.
    /// </summary>
    public sealed class NotFoundException :AppException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
