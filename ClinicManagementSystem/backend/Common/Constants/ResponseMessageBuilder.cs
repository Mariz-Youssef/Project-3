namespace ClinicManagementSystem.backend.Common.Constants
{
    /// <summary>
    /// Builds standardized response messages for application resources.
    /// </summary>
    public static class ResponseMessageBuilder
    {
        public static string Created(string resourceName) => $"{resourceName} created successfully.";
        public static string Updated(string resourceName) => $"{resourceName} updated successfully.";
        public static string Deleted(string resourceName) => $"{resourceName} deleted successfully.";
        public static string Retrieved(string resourceName) => $"{resourceName} retrieved successfully.";
        public static string RetrievedList(string resourceName) => $"{resourceName}  retrieved successfully.";
        public static string NotFound(string resourceName) => $"{resourceName} was not found.";
        public static string AlreadyExists(string resourceName) => $"{resourceName} already exists.";
    }
}
