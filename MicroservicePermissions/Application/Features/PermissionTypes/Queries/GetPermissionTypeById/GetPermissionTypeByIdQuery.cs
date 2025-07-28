using MediatR;
using MicroservicePermissions.Application.DTOs;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Queries.GetPermissionTypeById
{
    public class GetPermissionTypeByIdQuery : IRequest<PermissionTypeDto>
    {
        public int Id { get; set; }
        public GetPermissionTypeByIdQuery(int id)
        {
            Id = id;
        }
    }
}
