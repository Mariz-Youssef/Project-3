using System.ComponentModel.DataAnnotations;

namespace ClinicManagementSystem.backend.Common
{
    public abstract class BaseEntity : ISoftDeletable
    {
        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Indicates whether this entity has been soft deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the deletion date if the entity is soft deleted.
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
