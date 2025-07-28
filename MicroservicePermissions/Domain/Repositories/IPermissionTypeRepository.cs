using MicroservicePermissions.Domain.Entities;

namespace MicroservicePermissions.Domain.Repositories
{
    public interface IPermissionTypeRepository
    {
        Task<IEnumerable<PermissionType>> GetAllAsync();
        Task<PermissionType?> GetByIdAsync(int id);
        Task AddAsync(PermissionType permissionType);
        void Update(PermissionType permissionType);
        void Remove(PermissionType permissionType);
        Task<bool> ExistsAsync(int id);
    }
}
