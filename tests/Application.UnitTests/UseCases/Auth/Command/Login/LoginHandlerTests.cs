using Application.Common.Interfaces.Services;
using Application.UseCases.Auth.Command.Login;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.Login;

[TestFixture]
public class LoginHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private LoginHandler _loginHandler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _loginHandler = new LoginHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnLoginResponse()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        var expectedResponse = new LoginResponse
        {
            AccessToken = "access-token-123",
            RefreshToken = "refresh-token-456"
        };

        _internalApiServiceMock
            .Setup(x => x.LoginAsync(It.Is<LoginCommand>(c => 
                c.Email == command.Email && c.Password == command.Password), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _loginHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be(expectedResponse.AccessToken);
        result.RefreshToken.Should().Be(expectedResponse.RefreshToken);
        _internalApiServiceMock.Verify(
            x => x.LoginAsync(It.Is<LoginCommand>(c => 
                c.Email == command.Email && c.Password == command.Password), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiServiceWithCorrectParameters()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "user@test.com",
            Password = "SecurePassword123!"
        };

        var expectedResponse = new LoginResponse
        {
            AccessToken = "token",
            RefreshToken = "refresh"
        };

        _internalApiServiceMock
            .Setup(x => x.LoginAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        await _loginHandler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.LoginAsync(
                It.Is<LoginCommand>(c => c.Email == command.Email && c.Password == command.Password),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_ShouldPassCancellationTokenToService()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        var cancellationToken = new CancellationToken(true);
        var expectedResponse = new LoginResponse
        {
            AccessToken = "token",
            RefreshToken = "refresh"
        };

        _internalApiServiceMock
            .Setup(x => x.LoginAsync(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        await _loginHandler.Handle(command, cancellationToken);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.LoginAsync(
                It.IsAny<LoginCommand>(),
                It.Is<CancellationToken>(ct => ct == cancellationToken)),
            Times.Once);
    }
}
