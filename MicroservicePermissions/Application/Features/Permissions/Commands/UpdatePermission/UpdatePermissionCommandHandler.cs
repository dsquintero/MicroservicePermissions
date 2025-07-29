using MediatR;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKafkaProducer _kafkaProducer;

        public UpdatePermissionCommandHandler(IUnitOfWork unitOfWork, IKafkaProducer kafkaProducer)
        {
            _unitOfWork = unitOfWork;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<bool> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);

            if (permission == null)
                return false;

            var typeExists = await _unitOfWork.PermissionTypes.ExistsAsync(request.PermissionTypeId);
            if (!typeExists)
                return false;

            permission.EmployeeForename = request.EmployeeForename;
            permission.EmployeeSurname = request.EmployeeSurname;
            permission.PermissionTypeId = request.PermissionTypeId;
            permission.PermissionDate = request.PermissionDate;

            _unitOfWork.Permissions.Update(permission);
            await _unitOfWork.CompleteAsync();

            await _kafkaProducer.SendMessageAsync(new KafkaMessageDto<UpdatePermissionCommand>
            {
                Operation = "UpdatePermission",
                Data = request
            });

            return true;
        }
    }
}
