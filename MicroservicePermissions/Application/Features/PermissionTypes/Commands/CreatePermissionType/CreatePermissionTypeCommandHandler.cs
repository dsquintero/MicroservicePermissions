using AutoMapper;
using FluentValidation;
using MediatR;
using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Domain.Entities;

namespace MicroservicePermissions.Application.Features.PermissionTypes.Commands.CreatePermissionType
{
    public class CreatePermissionTypeCommandHandler : IRequestHandler<CreatePermissionTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreatePermissionTypeCommand> _validator;
        private readonly IMapper _mapper;

        public CreatePermissionTypeCommandHandler(IUnitOfWork unitOfWork, IValidator<CreatePermissionTypeCommand> validator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreatePermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                throw new ValidationException(string.Join(", ", errors));
            }

            var permissionType = _mapper.Map<PermissionType>(request);
            await _unitOfWork.PermissionTypes.AddAsync(permissionType);
            await _unitOfWork.CompleteAsync();

            return permissionType.Id;
        }
    }
}
