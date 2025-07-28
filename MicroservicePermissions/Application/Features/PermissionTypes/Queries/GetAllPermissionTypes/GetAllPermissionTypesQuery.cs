using MediatR;
using MicroservicePermissions.Application.DTOs;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Queries.GetAllPermissionTypes
{
    public class GetAllPermissionTypesQuery : IRequest<IEnumerable<PermissionTypeDto>>
    {
    }
}
