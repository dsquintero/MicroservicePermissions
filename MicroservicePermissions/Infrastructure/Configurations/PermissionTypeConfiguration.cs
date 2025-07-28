using MicroservicePermissions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicePermissions.Infrastructure.Configurations
{
    public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
    {
        public void Configure(EntityTypeBuilder<PermissionType> builder)
        {
            builder.ToTable("PermissionTypes");

            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.Description)
                   .IsRequired()
                   .HasMaxLength(100);

            // Seed data
            builder.HasData(
                new PermissionType { Id = 1, Description = "User" },
                new PermissionType { Id = 2, Description = "Admin" },
                new PermissionType { Id = 3, Description = "Root" }
            );
        }
    }
}
