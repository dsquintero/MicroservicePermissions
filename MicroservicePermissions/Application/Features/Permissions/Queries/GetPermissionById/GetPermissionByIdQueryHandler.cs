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

        public GetPermissionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IKafkaProducer kafkaProducer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<PermissionDto> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);

            if (permission == null)
                return null;

            await _kafkaProducer.SendMessageAsync(new KafkaMessageDto<GetPermissionByIdQuery>
            {
                Operation = "GetPermissionById",
                Data = request
            });

            return _mapper.Map<PermissionDto>(permission);
        }
    }
}
