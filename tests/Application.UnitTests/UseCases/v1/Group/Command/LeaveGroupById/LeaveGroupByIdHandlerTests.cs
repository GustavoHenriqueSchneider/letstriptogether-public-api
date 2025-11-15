using Application.Common.Interfaces.Services;
using Application.UseCases.v1.Group.Command.LeaveGroupById;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Group.Command.LeaveGroupById;

[TestFixture]
public class LeaveGroupByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private LeaveGroupByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new LeaveGroupByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new LeaveGroupByIdCommand { GroupId = Guid.NewGuid() };

        _internalApiServiceMock
            .Setup(x => x.LeaveGroupByIdAsync(It.IsAny<LeaveGroupByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.LeaveGroupByIdAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
