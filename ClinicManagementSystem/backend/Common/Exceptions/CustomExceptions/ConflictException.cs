namespace ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Thrown when a resource conflicts with existing data.
    /// </summary>
    public sealed class ConflictException :AppException
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
