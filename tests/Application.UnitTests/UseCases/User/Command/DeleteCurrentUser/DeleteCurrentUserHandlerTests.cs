using Application.Common.Interfaces.Services;
using Application.UseCases.User.Command.DeleteCurrentUser;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.User.Command.DeleteCurrentUser;

[TestFixture]
public class DeleteCurrentUserHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private DeleteCurrentUserHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new DeleteCurrentUserHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new DeleteCurrentUserCommand();

        _internalApiServiceMock
            .Setup(x => x.DeleteCurrentUserAsync(It.IsAny<DeleteCurrentUserCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.DeleteCurrentUserAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
