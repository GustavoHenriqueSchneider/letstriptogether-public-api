using Application.Common.Interfaces.Services;
using Application.UseCases.Auth.Command.ResetPassword;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.ResetPassword;

[TestFixture]
public class ResetPasswordHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private ResetPasswordHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new ResetPasswordHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new ResetPasswordCommand { Password = "NewPass123!" };

        _internalApiServiceMock
            .Setup(x => x.ResetPasswordAsync(It.IsAny<ResetPasswordCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.ResetPasswordAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
