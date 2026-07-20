using ClinicManagementSystem.backend.Data.Configurations.Base;
using ClinicManagementSystem.backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagementSystem.backend.Data.Configurations
{
    /// <summary>
    /// Configures the Department entity.
    /// Defines table mapping, property constraints,
    /// indexes, and relationships.
    /// </summary>
    public class DepartmentConfiguration : BaseEntityConfiguration<Department>
    {
        public override void Configure(EntityTypeBuilder<Department> builder)
        {
            base.Configure(builder);

            // ============================
            // Table Configuration
            // ============================

            builder.ToTable("Departments");

            // ============================
            // Primary Key
            // ============================

            builder.HasKey(department => department.Id);


            // ============================
            // Relationships
            // ============================

            /// One Department
            ///        │
            ///        ▼
            /// Many Doctors
            builder.HasMany(department => department.Doctors)
                   .WithOne(doctor => doctor.Department)
                   .HasForeignKey(doctor => doctor.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ============================
            // Indexes
            // ============================

            // Department names should be unique.
            builder.HasIndex(department => department.Name)
                   .IsUnique();
        }
    }
}
