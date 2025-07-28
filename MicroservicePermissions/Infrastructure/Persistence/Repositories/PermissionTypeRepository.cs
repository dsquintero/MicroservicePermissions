using MicroservicePermissions.Domain.Entities;
using MicroservicePermissions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MicroservicePermissions.Infrastructure.Persistence.Repositories
{
    public class PermissionTypeRepository : IPermissionTypeRepository
    {
        private readonly AppDbContext _context;

        public PermissionTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PermissionType>> GetAllAsync() =>
            await _context.PermissionTypes.ToListAsync();

        public async Task<PermissionType?> GetByIdAsync(int id) =>
            await _context.PermissionTypes.FirstOrDefaultAsync(pt => pt.Id == id);

        public async Task AddAsync(PermissionType permissionType) =>
            await _context.PermissionTypes.AddAsync(permissionType);

        public void Update(PermissionType permissionType) =>
            _context.PermissionTypes.Update(permissionType);

        public void Remove(PermissionType permissionType) =>
            _context.PermissionTypes.Remove(permissionType);
    }
}
