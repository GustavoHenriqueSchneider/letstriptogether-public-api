using Application.Common.Interfaces.Services;
using Application.UseCases.v1.Group.Command.DeleteGroupById;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Group.Command.DeleteGroupById;

[TestFixture]
public class DeleteGroupByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private DeleteGroupByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new DeleteGroupByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new DeleteGroupByIdCommand { GroupId = Guid.NewGuid() };

        _internalApiServiceMock
            .Setup(x => x.DeleteGroupByIdAsync(It.IsAny<DeleteGroupByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.DeleteGroupByIdAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
