using Application.Common.Interfaces.Services;
using Application.UseCases.v1.Auth.Command.Logout;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Auth.Command.Logout;

[TestFixture]
public class LogoutHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private LogoutHandler _logoutHandler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _logoutHandler = new LogoutHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new LogoutCommand();

        _internalApiServiceMock
            .Setup(x => x.LogoutAsync(It.IsAny<LogoutCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _logoutHandler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.LogoutAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_ShouldPassCancellationToken()
    {
        // Arrange
        var command = new LogoutCommand();
        var cancellationToken = new CancellationToken(true);

        _internalApiServiceMock
            .Setup(x => x.LogoutAsync(It.IsAny<LogoutCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _logoutHandler.Handle(command, cancellationToken);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.LogoutAsync(It.IsAny<LogoutCommand>(), cancellationToken),
            Times.Once);
    }
}
