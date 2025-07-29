using AutoMapper;
using MediatR;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Application.Features.Permissions.Queries.GetPermissionById
{
    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IElasticPermissionIndexer _elasticIndexer;

        public GetPermissionByIdQueryHandler(
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

        public async Task<PermissionDto> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);

            if (permission == null)
                return null;
            string operation = "GetPermissionById";
            await _kafkaProducer.SendMessageAsync(new KafkaMessageDto<GetPermissionByIdQuery>
            {
                Operation = operation,
                Data = request
            });

            var permissionElasticDto = _mapper.Map<PermissionElasticDto>(permission);
            await _elasticIndexer.IndexAsync(permissionElasticDto, operation.ToLower());

            return _mapper.Map<PermissionDto>(permission);
        }
    }
}
