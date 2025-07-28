using MediatR;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Commands.CreatePermissionType
{
    public class CreatePermissionTypeCommand : IRequest<int>
    {
        public string Description { get; set; } = string.Empty;
    }
}
