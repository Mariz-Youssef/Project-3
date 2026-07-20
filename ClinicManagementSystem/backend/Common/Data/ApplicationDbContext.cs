using ClinicManagementSystem.backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace ClinicManagementSystem.backend.Common.Data
{
    /// <summary>
    /// Represents the application's database context.
    /// Manages Identity tables and business entities.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        //Making the Dbsets for the business entities
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<DoctorWorkingHour> DoctorWorkingHours => Set<DoctorWorkingHour>();
        public DbSet<DoctorLeave> DoctorLeaves => Set<DoctorLeave>();

        /// <summary>Gets the refresh tokens issued across the system.</summary>
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all Fluent API configurations automatically
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global query filters for soft delete.
            ApplySoftDeleteQueryFilter(builder);
        }


        #region SaveChanges

        public override int SaveChanges()
        {
            ApplyAuditInformation();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();

            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Automatically updates audit fields
        /// and converts hard delete operations
        /// into soft delete operations.
        /// </summary>
        private void ApplyAuditInformation()
        {
            foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:

                        if (entry.Entity is BaseEntity addedEntity)
                        {
                            addedEntity.CreatedAt = DateTime.UtcNow;
                        }

                        break;

                    case EntityState.Modified:

                        if (entry.Entity is BaseEntity modifiedEntity)
                        {
                            modifiedEntity.UpdatedAt = DateTime.UtcNow;
                        }

                        break;

                    case EntityState.Deleted:

                        entry.State = EntityState.Modified;

                        entry.Entity.IsDeleted = true;

                        entry.Entity.DeletedAt = DateTime.UtcNow;

                        if (entry.Entity is BaseEntity deletedEntity)
                        {
                            deletedEntity.UpdatedAt = DateTime.UtcNow;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Applies a global query filter to
        /// automatically exclude soft deleted entities.
        /// </summary>
        private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                    continue;

                var parameter = Expression.Parameter(entityType.ClrType, "entity");

                var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));

                var compareExpression = Expression.Equal(property, Expression.Constant(false));

                var lambda = Expression.Lambda(compareExpression, parameter);

                modelBuilder.Entity(entityType.ClrType)
                            .HasQueryFilter(lambda);
            }
        }

        #endregion

    }
}
