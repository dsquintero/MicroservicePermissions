using MicroservicePermissions.Domain.Entities;

namespace MicroservicePermissions.Domain.Repositories
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(int id);
        Task AddAsync(Permission permission);
        void Update(Permission permission);
        void Remove(Permission permission);
    }
}
