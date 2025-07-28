using MicroservicePermissions.Domain.Entities;
using MicroservicePermissions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroservicePermissions.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;

        public PermissionRepository(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync() =>
       await _context.Permissions.Include(p => p.PermissionType).ToListAsync();

        public async Task<Permission?> GetByIdAsync(int id) =>
            await _context.Permissions.Include(p => p.PermissionType).FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Permission permission) => await _context.Permissions.AddAsync(permission);

        public void Update(Permission permission) => _context.Permissions.Update(permission);

        public void Remove(Permission permission) => _context.Permissions.Remove(permission);
    }
}
