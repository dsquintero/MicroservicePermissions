using MediatR;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Commands.DeletePermissionType
{
    public class DeletePermissionTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeletePermissionTypeCommand(int id)
        {
            Id = id;
        }
    }
}
