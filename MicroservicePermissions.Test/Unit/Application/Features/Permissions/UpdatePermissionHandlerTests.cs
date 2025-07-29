using AutoMapper;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Features.Permissions.Commands.UpdatePermission;
using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Domain.Entities;
using Moq;

namespace MicroservicePermissions.Test.Unit.Application.Features.Permissions;

[TestFixture]
public class UpdatePermissionHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IKafkaProducer> _kafkaProducerMock;
    private Mock<IElasticPermissionIndexer> _elasticIndexerMock;
    private UpdatePermissionCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _elasticIndexerMock = new Mock<IElasticPermissionIndexer>();

        _handler = new UpdatePermissionCommandHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _kafkaProducerMock.Object,
            _elasticIndexerMock.Object
        );
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldUpdatePermissionAndReturnTrue()
    {
        // Arrange
        var command = new UpdatePermissionCommand
        {
            Id = 1,
            EmployeeForename = "UpdatedName",
            EmployeeSurname = "UpdatedSurname",
            PermissionTypeId = 2,
            PermissionDate = DateTime.UtcNow
        };

        var permission = new Permission
        {
            Id = 1,
            EmployeeForename = "OldName",
            EmployeeSurname = "OldSurname",
            PermissionTypeId = 1,
            PermissionDate = DateTime.UtcNow.AddDays(-1)
        };

        var elasticDto = new PermissionElasticDto();

        _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(command.Id)).ReturnsAsync(permission);
        _unitOfWorkMock.Setup(u => u.PermissionTypes.ExistsAsync(command.PermissionTypeId)).ReturnsAsync(true);
        _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

        _mapperMock.Setup(m => m.Map<PermissionElasticDto>(It.IsAny<Permission>())).Returns(elasticDto);
        _kafkaProducerMock.Setup(k => k.SendMessageAsync(It.IsAny<KafkaMessageDto<UpdatePermissionCommand>>())).Returns(Task.CompletedTask);
        _elasticIndexerMock.Setup(e => e.IndexAsync(elasticDto, "updatepermission")).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        _unitOfWorkMock.Verify(u => u.Permissions.Update(It.IsAny<Permission>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_PermissionNotFound_ShouldReturnFalse()
    {
        // Arrange
        var command = new UpdatePermissionCommand { Id = 999 };
        _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(command.Id)).ReturnsAsync((Permission?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsFalse(result);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Never);
    }

    [Test]
    public async Task Handle_PermissionTypeDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var command = new UpdatePermissionCommand
        {
            Id = 1,
            PermissionTypeId = 99
        };

        var permission = new Permission { Id = 1 };

        _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(command.Id)).ReturnsAsync(permission);
        _unitOfWorkMock.Setup(u => u.PermissionTypes.ExistsAsync(command.PermissionTypeId)).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsFalse(result);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Never);
    }
}
