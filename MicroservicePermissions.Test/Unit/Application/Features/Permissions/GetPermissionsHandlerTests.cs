using AutoMapper;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Features.Permissions.Queries.GetAllPermissions;
using MicroservicePermissions.Application.Features.Permissions.Queries.GetPermissionById;
using MicroservicePermissions.Application.Interfaces;
using MicroservicePermissions.Domain.Entities;
using Moq;

namespace MicroservicePermissions.Test.Unit.Application.Features.Permissions;

[TestFixture]
public class GetPermissionsHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IKafkaProducer> _kafkaProducerMock;
    private Mock<IElasticPermissionIndexer> _elasticIndexerMock;

    [SetUp]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _elasticIndexerMock = new Mock<IElasticPermissionIndexer>();
    }

    [Test]
    public async Task GetPermissionById_PermissionExists_ReturnsPermissionDto()
    {
        // Arrange
        var query = new GetPermissionByIdQuery(1);
        var permission = new Permission { Id = 1 };
        var dto = new PermissionDto { Id = 1 };
        var elasticDto = new PermissionElasticDto();

        _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(1)).ReturnsAsync(permission);
        _mapperMock.Setup(m => m.Map<PermissionDto>(permission)).Returns(dto);
        _mapperMock.Setup(m => m.Map<PermissionElasticDto>(permission)).Returns(elasticDto);
        _kafkaProducerMock.Setup(k => k.SendMessageAsync(It.IsAny<KafkaMessageDto<GetPermissionByIdQuery>>()))
                          .Returns(Task.CompletedTask);
        _elasticIndexerMock.Setup(e => e.IndexAsync(elasticDto, "getpermissionbyid")).Returns(Task.CompletedTask);

        var handler = new GetPermissionByIdQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _kafkaProducerMock.Object,
            _elasticIndexerMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(1));
        _kafkaProducerMock.Verify(k => k.SendMessageAsync(It.IsAny<KafkaMessageDto<GetPermissionByIdQuery>>()), Times.Once);
        _elasticIndexerMock.Verify(e => e.IndexAsync(elasticDto, "getpermissionbyid"), Times.Once);
    }

    [Test]
    public async Task GetPermissionById_PermissionDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetPermissionByIdQuery(99);
        _unitOfWorkMock.Setup(u => u.Permissions.GetByIdAsync(query.Id)).ReturnsAsync((Permission?)null);

        var handler = new GetPermissionByIdQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _kafkaProducerMock.Object,
            _elasticIndexerMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsNull(result);
        _kafkaProducerMock.Verify(k => k.SendMessageAsync(It.IsAny<KafkaMessageDto<GetPermissionByIdQuery>>()), Times.Never);
        _elasticIndexerMock.Verify(e => e.IndexAsync(It.IsAny<PermissionElasticDto>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task GetAllPermissions_ReturnsMappedList()
    {
        // Arrange
        var query = new GetAllPermissionsQuery();
        var permissions = new List<Permission>
        {
            new Permission { Id = 1 },
            new Permission { Id = 2 }
        };

        var permissionDtos = new List<PermissionDto>
        {
            new PermissionDto { Id = 1 },
            new PermissionDto { Id = 2 }
        };

        _unitOfWorkMock.Setup(u => u.Permissions.GetAllAsync()).ReturnsAsync(permissions);
        _mapperMock.Setup(m => m.Map<IEnumerable<PermissionDto>>(permissions)).Returns(permissionDtos);
        _kafkaProducerMock.Setup(k => k.SendMessageAsync(It.IsAny<KafkaMessageDto<GetAllPermissionsQuery>>()))
                          .Returns(Task.CompletedTask);

        var handler = new GetAllPermissionsQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _kafkaProducerMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        _kafkaProducerMock.Verify(k => k.SendMessageAsync(It.IsAny<KafkaMessageDto<GetAllPermissionsQuery>>()), Times.Once);
    }
}
