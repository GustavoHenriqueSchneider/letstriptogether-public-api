using Application.Common.Interfaces.Services;
using Application.UseCases.Auth.Command.Register;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.Register;

[TestFixture]
public class RegisterHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private RegisterHandler _registerHandler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _registerHandler = new RegisterHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnRegisterResponse()
    {
        // Arrange
        var command = new RegisterCommand { Password = "ValidPass123!", HasAcceptedTermsOfUse = true };
        var expectedResponse = new RegisterResponse { Id = Guid.NewGuid() };

        _internalApiServiceMock
            .Setup(x => x.RegisterAsync(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _registerHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.RegisterAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
