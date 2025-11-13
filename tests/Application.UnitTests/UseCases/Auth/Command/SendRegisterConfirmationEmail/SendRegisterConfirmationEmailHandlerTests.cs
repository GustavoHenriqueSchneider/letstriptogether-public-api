using Application.Common.Interfaces.Services;
using Application.UseCases.Auth.Command.SendRegisterConfirmationEmail;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.SendRegisterConfirmationEmail;

[TestFixture]
public class SendRegisterConfirmationEmailHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private SendRegisterConfirmationEmailHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new SendRegisterConfirmationEmailHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnResponse()
    {
        // Arrange
        var command = new SendRegisterConfirmationEmailCommand { Name = "John", Email = "john@test.com" };
        var expectedResponse = new SendRegisterConfirmationEmailResponse { Token = "confirmation-token" };

        _internalApiServiceMock
            .Setup(x => x.SendRegisterConfirmationEmailAsync(It.IsAny<SendRegisterConfirmationEmailCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.SendRegisterConfirmationEmailAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
