using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities.Concrete;

namespace DataAccess.Concrete.Configurations
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Define the primary key (inherited from BaseEntity)
            builder.HasKey(r => r.Id);

            // Define column configurations (e.g., maximum length for string properties)
            builder.Property(r => r.Name)
                .IsRequired()  // Make sure Name is required
                .HasMaxLength(100);  // Limit the Name column length to 100 characters

            builder.Property(r => r.Description)
                .IsRequired(false)  // Description is optional
                .HasMaxLength(255);  // Limit the Description column length to 255 characters

            // You can add more configurations as needed, for example:
            // - Indexes
            // - Relationships
            // - Default values
            // - etc.

            // Optionally, seed data for the Roles table (Admin, School, Student, Parent, Trainer)
            builder.HasData(
                new Role { Id = 1, Name = "Admin", Description = "Administrator with full access" },
                new Role { Id = 2, Name = "School", Description = "Role for school management" },
                new Role { Id = 3, Name = "Student", Description = "Role for students" },
                new Role { Id = 4, Name = "Parent", Description = "Role for parents" },
                new Role { Id = 5, Name = "Trainer", Description = "Role for trainers or instructors" }
            );
        }
    }
}