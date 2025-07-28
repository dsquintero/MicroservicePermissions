using MediatR;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Commands.DeletePermissionType
{
    public class DeletePermissionTypeCommandHandler : IRequestHandler<DeletePermissionTypeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePermissionTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var permissionType = await _unitOfWork.PermissionTypes.GetByIdAsync(request.Id);

            if (permissionType == null)
                return false;

            _unitOfWork.PermissionTypes.Remove(permissionType);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
