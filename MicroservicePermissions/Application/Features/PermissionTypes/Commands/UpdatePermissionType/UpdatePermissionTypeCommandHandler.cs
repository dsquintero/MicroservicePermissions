using MediatR;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Commands.UpdatePermissionType
{
    public class UpdatePermissionTypeCommandHandler : IRequestHandler<UpdatePermissionTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePermissionTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdatePermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var permissionType = await _unitOfWork.PermissionTypes.GetByIdAsync(request.Id);

            if (permissionType == null)
                return false;

            permissionType.Description = request.Description;

            _unitOfWork.PermissionTypes.Update(permissionType);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
