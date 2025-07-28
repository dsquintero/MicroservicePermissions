using FluentValidation;
using MicroservicePermissions.Domain.Repositories;

namespace MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionCommandValidator(IPermissionTypeRepository permissionTypeRepository)
        {
            RuleFor(x => x.EmployeeForename).NotEmpty();
            RuleFor(x => x.EmployeeSurname).NotEmpty();
            RuleFor(x => x.PermissionTypeId).GreaterThan(0);
            RuleFor(x => x.PermissionDate).NotEmpty();
            RuleFor(x => x.PermissionTypeId)
            .MustAsync(async (id, _) => await permissionTypeRepository.ExistsAsync(id))
            .WithMessage("El tipo de permiso especificado no existe.");
        }
    }
}
