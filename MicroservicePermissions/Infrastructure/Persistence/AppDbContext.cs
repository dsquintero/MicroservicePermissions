using MicroservicePermissions.Domain.Entities;
using MicroservicePermissions.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MicroservicePermissions.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PermissionTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
