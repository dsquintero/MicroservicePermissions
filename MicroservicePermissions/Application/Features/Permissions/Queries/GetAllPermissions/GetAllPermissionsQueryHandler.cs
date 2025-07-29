using AutoMapper;
using MediatR;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Application.Features.Permissions.Queries.GetAllPermissions
{
    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, IEnumerable<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IKafkaProducer _kafkaProducer;

        public GetAllPermissionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IKafkaProducer kafkaProducer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _unitOfWork.Permissions.GetAllAsync();
            await _kafkaProducer.SendMessageAsync(new KafkaMessageDto<GetAllPermissionsQuery>
            {
                Operation = "GetAllPermissions",
                Data = request
            });

            return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
        }
    }
}
