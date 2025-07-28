using AutoMapper;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission;
using MicroservicePermissions.Application.Features.PermissionTypes.Commands.CreatePermissionType;
using MicroservicePermissions.Domain.Entities;

namespace MicroservicePermissions.Application.Mappings
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<CreatePermissionCommand, Permission>();
            CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.PermissionTypeDescription,
                           opt => opt.MapFrom(src => src.PermissionType.Description));
            CreateMap<CreatePermissionTypeCommand, PermissionType>();
            CreateMap<PermissionType, PermissionTypeDto>();
        }
    }
}
