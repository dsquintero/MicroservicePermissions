using AutoMapper;
using MediatR;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Queries.GetPermissionTypeById
{
    public class GetPermissionTypeByIdQueryHandler : IRequestHandler<GetPermissionTypeByIdQuery, PermissionTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPermissionTypeByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PermissionTypeDto> Handle(GetPermissionTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var permissionType = await _unitOfWork.PermissionTypes.GetByIdAsync(request.Id);

            if (permissionType == null)
                return null;

            return _mapper.Map<PermissionTypeDto>(permissionType);
        }
    }
}
