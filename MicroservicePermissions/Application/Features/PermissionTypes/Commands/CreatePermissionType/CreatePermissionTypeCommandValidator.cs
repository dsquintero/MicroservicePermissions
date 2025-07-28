using FluentValidation;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Commands.CreatePermissionType
{
    public class CreatePermissionTypeCommandValidator : AbstractValidator<CreatePermissionTypeCommand>
    {
        public CreatePermissionTypeCommandValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(100).WithMessage("La descripción no puede superar los 100 caracteres.");
        }
    }
}
