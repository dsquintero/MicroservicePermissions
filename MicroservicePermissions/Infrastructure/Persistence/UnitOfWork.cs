using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Domain.Repositories;

namespace MicroservicePermissions.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IPermissionRepository Permissions { get; }

        public UnitOfWork(AppDbContext context, IPermissionRepository permission)
        {
            this._context = context;
            this.Permissions = permission;
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
