using Application.Common.Interfaces.Services;
using Application.UseCases.Auth.Command.ValidateRegisterConfirmationCode;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.ValidateRegisterConfirmationCode;

[TestFixture]
public class ValidateRegisterConfirmationCodeHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private ValidateRegisterConfirmationCodeHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new ValidateRegisterConfirmationCodeHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnResponse()
    {
        // Arrange
        var command = new ValidateRegisterConfirmationCodeCommand { Code = 123456 };
        var expectedResponse = new ValidateRegisterConfirmationCodeResponse { Token = "validation-token" };

        _internalApiServiceMock
            .Setup(x => x.ValidateRegisterConfirmationCodeAsync(It.IsAny<ValidateRegisterConfirmationCodeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.ValidateRegisterConfirmationCodeAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
