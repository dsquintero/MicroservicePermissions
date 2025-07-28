using AutoMapper;
using FluentValidation;
using MediatR;
using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Domain.Entities;

namespace MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionHandler : IRequestHandler<CreatePermissionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePermissionCommand> _validator;

        public CreatePermissionHandler(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreatePermissionCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
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

            return permission.Id;
        }
    }
}
