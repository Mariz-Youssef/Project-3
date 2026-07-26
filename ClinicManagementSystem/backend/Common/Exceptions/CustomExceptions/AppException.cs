namespace ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions
{
    /// <summary>
    /// Represents the base exception for all application-specific exceptions.
    /// </summary>
    public abstract class AppException:Exception
    {
        protected AppException(string message)
            : base(message)
        {
        }
    }
}
