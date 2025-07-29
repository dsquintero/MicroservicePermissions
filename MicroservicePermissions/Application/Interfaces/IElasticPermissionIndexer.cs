using MicroservicePermissions.Application.DTOs;

namespace MicroservicePermissions.Application.Interfaces
{
    public interface IElasticPermissionIndexer
    {
        Task IndexAsync(PermissionElasticDto dto, string _indexName);
    }
}
