using FluentValidation;

namespace MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionCommandValidator()
        {
            RuleFor(x => x.EmployeeForename).NotEmpty();
            RuleFor(x => x.EmployeeSurname).NotEmpty();
            RuleFor(x => x.PermissionTypeId).GreaterThan(0);
            RuleFor(x => x.PermissionDate).NotEmpty();
        }
    }
}
