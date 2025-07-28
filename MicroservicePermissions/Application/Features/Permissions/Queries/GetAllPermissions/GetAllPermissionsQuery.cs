using MediatR;
using MicroservicePermissions.Application.DTOs;

namespace MicroservicePermissions.Application.Features.Permissions.Queries.GetAllPermissions
{
    public class GetAllPermissionsQuery : IRequest<IEnumerable<PermissionDto>>
    {
    }
}
