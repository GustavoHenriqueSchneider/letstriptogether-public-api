using Application.Common.Interfaces.Services;
using Application.UseCases.v1.Auth.Command.RequestResetPassword;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Auth.Command.RequestResetPassword;

[TestFixture]
public class RequestResetPasswordHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private RequestResetPasswordHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new RequestResetPasswordHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new RequestResetPasswordCommand { Email = "test@test.com" };

        _internalApiServiceMock
            .Setup(x => x.RequestResetPasswordAsync(It.IsAny<RequestResetPasswordCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.RequestResetPasswordAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
