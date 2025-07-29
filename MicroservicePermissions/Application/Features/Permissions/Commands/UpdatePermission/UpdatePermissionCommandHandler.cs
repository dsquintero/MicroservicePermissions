using AutoMapper;
using MediatR;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Application.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IElasticPermissionIndexer _elasticIndexer;

        public UpdatePermissionCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IKafkaProducer kafkaProducer,
            IElasticPermissionIndexer elasticIndexer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _kafkaProducer = kafkaProducer;
            _elasticIndexer = elasticIndexer;
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
            string operation = "UpdatePermission";
            await _kafkaProducer.SendMessageAsync(new KafkaMessageDto<UpdatePermissionCommand>
            {
                Operation = operation,
                Data = request
            });

            var permissionElasticDto = _mapper.Map<PermissionElasticDto>(permission);
            await _elasticIndexer.IndexAsync(permissionElasticDto, operation.ToLower());

            return true;
        }
    }
}
