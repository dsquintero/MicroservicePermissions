using MediatR;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Application.Features.Permissions.Commands.DeletePermission
{
    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKafkaProducer _kafkaProducer;

        public DeletePermissionCommandHandler(IUnitOfWork unitOfWork, IKafkaProducer kafkaProducer)
        {
            _unitOfWork = unitOfWork;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<bool> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);
            if (permission == null)
                return false;

            _unitOfWork.Permissions.Remove(permission);
            await _unitOfWork.CompleteAsync();

            await _kafkaProducer.SendMessageAsync(new KafkaMessageDto<DeletePermissionCommand>
            {
                Operation = "DeletePermission",
                Data = request
            });

            return true;
        }
    }
}
