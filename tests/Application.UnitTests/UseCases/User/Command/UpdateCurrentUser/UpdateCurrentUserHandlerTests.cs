using Application.Common.Interfaces.Services;
using Application.UseCases.User.Command.UpdateCurrentUser;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.User.Command.UpdateCurrentUser;

[TestFixture]
public class UpdateCurrentUserHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private UpdateCurrentUserHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new UpdateCurrentUserHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new UpdateCurrentUserCommand { Name = "New Name" };

        _internalApiServiceMock
            .Setup(x => x.UpdateCurrentUserAsync(It.IsAny<UpdateCurrentUserCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.UpdateCurrentUserAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
