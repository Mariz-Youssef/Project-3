using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Common.Data.Configurations.Base
{
    /// <summary>
    /// Provides common Fluent API configurations
    /// for all entities that inherit from BaseEntity.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ConfigureBaseProperties(builder);
        }

        /// <summary>
        /// Configures common properties shared
        /// by all entities.
        /// </summary>
        protected void ConfigureBaseProperties(EntityTypeBuilder<TEntity> builder)
        {
            // Configure CreatedAt default value.
            builder.Property(entity => entity.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // Configure IsDeleted default value.
            builder.Property(entity => entity.IsDeleted)
                   .HasDefaultValue(false);
        }
    }
}
