using MediatR;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Commands.UpdatePermissionType
{
    public class UpdatePermissionTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
