using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Domain.Repositories;

namespace MicroservicePermissions.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IPermissionRepository Permissions { get; }
        public IPermissionTypeRepository PermissionTypes { get; }

        public UnitOfWork(
            AppDbContext context,
            IPermissionRepository permissionRepository,
            IPermissionTypeRepository permissionTypeRepository)
        {
            _context = context;
            Permissions = permissionRepository;
            PermissionTypes = permissionTypeRepository;
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
