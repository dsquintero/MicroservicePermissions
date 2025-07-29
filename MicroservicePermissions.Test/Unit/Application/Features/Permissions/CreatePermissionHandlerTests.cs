using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Features.Permissions.Commands.CreatePermission;
using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Domain.Entities;
using Moq;

namespace MicroservicePermissions.Test.Unit.Application.Features.Permissions;

[TestFixture]
public class CreatePermissionHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IValidator<CreatePermissionCommand>> _validatorMock;
    private Mock<IKafkaProducer> _kafkaProducerMock;
    private Mock<IElasticPermissionIndexer> _elasticIndexerMock;
    private Mock<IMapper> _mapperMock;
    private CreatePermissionHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<CreatePermissionCommand>>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _elasticIndexerMock = new Mock<IElasticPermissionIndexer>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreatePermissionHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _validatorMock.Object,
            _kafkaProducerMock.Object,
            _elasticIndexerMock.Object
        );
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldCreatePermissionAndReturnId()
    {
        // Arrange
        var command = new CreatePermissionCommand
        {
            EmployeeForename = "John",
            EmployeeSurname = "Doe",
            PermissionTypeId = 1,
            PermissionDate = DateTime.UtcNow
        };

        var permission = new Permission { Id = 1 };
        var elasticDto = new PermissionElasticDto();

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _mapperMock.Setup(m => m.Map<Permission>(command)).Returns(permission);
        _mapperMock.Setup(m => m.Map<PermissionElasticDto>(permission)).Returns(elasticDto);

        _unitOfWorkMock.Setup(u => u.Permissions.AddAsync(permission)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

        _kafkaProducerMock.Setup(k => k.SendMessageAsync(It.IsAny<KafkaMessageDto<CreatePermissionCommand>>()))
            .Returns(Task.CompletedTask);

        _elasticIndexerMock.Setup(e => e.IndexAsync(elasticDto, "createpermission"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(1));
        _unitOfWorkMock.Verify(u => u.Permissions.AddAsync(permission), Times.Once);
        _kafkaProducerMock.Verify(k => k.SendMessageAsync(It.IsAny<KafkaMessageDto<CreatePermissionCommand>>()), Times.Once);
        _elasticIndexerMock.Verify(e => e.IndexAsync(elasticDto, "createpermission"), Times.Once);
    }

    [Test]
    public void Handle_InvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreatePermissionCommand();
        var validationResult = new FluentValidation.Results.ValidationResult(new[] {
                new ValidationFailure("EmployeeForename", "Required")
            });

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act & Assert
        var ex = Assert.ThrowsAsync<FluentValidation.ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.That(ex.Message, Does.Contain("EmployeeForename"));
    }
}
