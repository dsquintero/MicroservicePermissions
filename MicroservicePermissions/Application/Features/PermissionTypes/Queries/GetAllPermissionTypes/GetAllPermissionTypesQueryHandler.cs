using AutoMapper;
using MediatR;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Queries.GetAllPermissionTypes
{
    public class GetAllPermissionTypesQueryHandler : IRequestHandler<GetAllPermissionTypesQuery, IEnumerable<PermissionTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPermissionTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionTypeDto>> Handle(GetAllPermissionTypesQuery request, CancellationToken cancellationToken)
        {
            var permissionTypes = await _unitOfWork.PermissionTypes.GetAllAsync();

            return _mapper.Map<IEnumerable<PermissionTypeDto>>(permissionTypes);
        }
    }
}
