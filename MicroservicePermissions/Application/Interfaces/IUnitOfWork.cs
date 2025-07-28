using MicroservicePermissions.Domain.Repositories;

namespace MicroservicePermissions.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPermissionRepository Permissions { get; }
        IPermissionTypeRepository PermissionTypes { get; }
        Task<int> CompleteAsync();
    }
}
