using Application.Common.Interfaces.Services;
using Application.UseCases.User.Command.ChangeCurrentUserPassword;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.User.Command.ChangeCurrentUserPassword;

[TestFixture]
public class ChangeCurrentUserPasswordHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private ChangeCurrentUserPasswordHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new ChangeCurrentUserPasswordHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "CurrentPass123!",
            NewPassword = "NewPass123!"
        };

        _internalApiServiceMock
            .Setup(x => x.ChangeCurrentUserPasswordAsync(It.IsAny<ChangeCurrentUserPasswordCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.ChangeCurrentUserPasswordAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_ShouldPassCancellationToken()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "CurrentPass123!",
            NewPassword = "NewPass123!"
        };
        var cancellationToken = new CancellationToken(true);

        _internalApiServiceMock
            .Setup(x => x.ChangeCurrentUserPasswordAsync(It.IsAny<ChangeCurrentUserPasswordCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.ChangeCurrentUserPasswordAsync(It.IsAny<ChangeCurrentUserPasswordCommand>(), cancellationToken),
            Times.Once);
    }

    [Test]
    public async Task Handle_ShouldPassCorrectCommandToService()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "CurrentPass123!",
            NewPassword = "NewPass123!"
        };

        _internalApiServiceMock
            .Setup(x => x.ChangeCurrentUserPasswordAsync(It.Is<ChangeCurrentUserPasswordCommand>(c =>
                c.CurrentPassword == command.CurrentPassword && c.NewPassword == command.NewPassword),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.ChangeCurrentUserPasswordAsync(
                It.Is<ChangeCurrentUserPasswordCommand>(c =>
                    c.CurrentPassword == command.CurrentPassword &&
                    c.NewPassword == command.NewPassword),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
