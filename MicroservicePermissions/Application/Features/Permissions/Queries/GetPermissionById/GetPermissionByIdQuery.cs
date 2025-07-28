using MediatR;
using MicroservicePermissions.Application.DTOs;

namespace MicroservicePermissions.Application.Features.Permissions.Queries.GetPermissionById
{
    public class GetPermissionByIdQuery : IRequest<PermissionDto>
    {
        public int Id { get; set; }

        public GetPermissionByIdQuery(int id)
        {
            Id = id;
        }
    }
}
