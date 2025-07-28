using MediatR;

namespace MicroservicePermissions.Application.Features.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommand : IRequest<bool> // Devuelve true si fue eliminado
    {
        public int Id { get; set; }

        public DeletePermissionCommand(int id)
        {
            Id = id;
        }
    }
}
