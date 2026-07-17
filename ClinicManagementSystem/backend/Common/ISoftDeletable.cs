namespace ClinicManagementSystem.backend.Common
{
    /// <summary>
    /// Represents an entity that supports soft deletion.
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// Indicates whether the entity has been soft deleted.
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// The date and time when the entity was deleted.
        /// </summary>
        DateTime? DeletedAt { get; set; }
    }
}
