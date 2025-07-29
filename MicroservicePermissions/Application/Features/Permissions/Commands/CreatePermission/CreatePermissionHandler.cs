using AutoMapper;
using FluentValidation;
using MediatR;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Domain.Entities;

namespace MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionHandler : IRequestHandler<CreatePermissionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePermissionCommand> _validator;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IElasticPermissionIndexer _elasticIndexer;

        public CreatePermissionHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreatePermissionCommand> validator,
            IKafkaProducer kafkaProducer,
            IElasticPermissionIndexer elasticIndexer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _kafkaProducer = kafkaProducer;
            _elasticIndexer = elasticIndexer;
        }

        public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                throw new ValidationException(string.Join(", ", errors));
            }

            var permission = _mapper.Map<Permission>(request);
            await _unitOfWork.Permissions.AddAsync(permission);
            await _unitOfWork.CompleteAsync();

            await _kafkaProducer.SendMessageAsync(new KafkaMessageDto<CreatePermissionCommand>
            {
                Operation = "CreatePermission",
                Data = request
            });

            var permissionElasticDto = _mapper.Map<PermissionElasticDto>(permission);
            await _elasticIndexer.IndexAsync(permissionElasticDto);

            return permission.Id;
        }
    }
}
