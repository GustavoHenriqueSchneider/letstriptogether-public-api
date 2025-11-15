using System.Security.Claims;
using Application.Common.Extensions;
using Application.Common.Interfaces.Extensions;
using Domain.Security;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApi.Hubs;

namespace WebApi.UnitTests.Hubs;

[TestFixture]
public class NotificationHubTests
{
    private Mock<ILogger<NotificationHub>> _loggerMock = null!;
    private Mock<HubCallerContext> _contextMock = null!;
    private Mock<IGroupManager> _groupsMock = null!;
    private Mock<IHubCallerClients> _clientsMock = null!;
    private NotificationHub _hub = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<NotificationHub>>();
        _contextMock = new Mock<HubCallerContext>();
        _groupsMock = new Mock<IGroupManager>();
        _clientsMock = new Mock<IHubCallerClients>();
        
        _hub = new NotificationHub(_loggerMock.Object)
        {
            Context = _contextMock.Object,
            Groups = _groupsMock.Object,
            Clients = _clientsMock.Object
        };
    }

    [TearDown]
    public void TearDown()
    {
        _hub?.Dispose();
    }


    [Test]
    public async Task OnConnectedAsync_WhenTokenTypeIsInvalid_ShouldAbortConnection()
    {
        // Arrange
        var connectionId = "test-connection-id";
        var claims = new List<Claim>
        {
            new(Claims.Id, Guid.NewGuid().ToString()),
            new(Claims.TokenType, "invalid")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        
        _contextMock.Setup(x => x.User).Returns(principal);
        _contextMock.Setup(x => x.ConnectionId).Returns(connectionId);
        _contextMock.Setup(x => x.Abort()).Verifiable();

        // Act
        await _hub.OnConnectedAsync();

        // Assert
        _contextMock.Verify(x => x.Abort(), Times.Once);
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Warning) &&
            i.Arguments[2].ToString()!.Contains("Invalid token type"));
    }

    [Test]
    public async Task OnConnectedAsync_WhenUserIdIsEmpty_ShouldAbortConnection()
    {
        // Arrange
        var connectionId = "test-connection-id";
        var claims = new List<Claim>
        {
            new(Claims.Id, Guid.Empty.ToString()),
            new(Claims.TokenType, TokenTypes.Access)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        
        _contextMock.Setup(x => x.User).Returns(principal);
        _contextMock.Setup(x => x.ConnectionId).Returns(connectionId);
        _contextMock.Setup(x => x.Abort()).Verifiable();

        // Act
        await _hub.OnConnectedAsync();

        // Assert
        _contextMock.Verify(x => x.Abort(), Times.Once);
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Warning) &&
            i.Arguments[2].ToString()!.Contains("User connected without valid Id claim"));
    }

    [Test]
    public async Task OnConnectedAsync_WhenValid_ShouldAddToGroup()
    {
        // Arrange
        var connectionId = "test-connection-id";
        var userId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new(Claims.Id, userId.ToString()),
            new(Claims.TokenType, TokenTypes.Access)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        
        _contextMock.Setup(x => x.User).Returns(principal);
        _contextMock.Setup(x => x.ConnectionId).Returns(connectionId);
        _groupsMock.Setup(x => x.AddToGroupAsync(connectionId, $"user_{userId}", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _hub.OnConnectedAsync();

        // Assert
        _groupsMock.Verify(x => x.AddToGroupAsync(connectionId, $"user_{userId}", It.IsAny<CancellationToken>()), Times.Once);
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Information) &&
            i.Arguments[2].ToString()!.Contains("User") &&
            i.Arguments[2].ToString()!.Contains("connected to NotificationHub"));
    }

    [Test]
    public async Task OnDisconnectedAsync_WhenUserIdIsEmpty_ShouldLogWarning()
    {
        // Arrange
        var connectionId = "test-connection-id";
        var claims = new List<Claim>
        {
            new(Claims.Id, Guid.Empty.ToString()),
            new(Claims.TokenType, TokenTypes.Access)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        
        _contextMock.Setup(x => x.User).Returns(principal);
        _contextMock.Setup(x => x.ConnectionId).Returns(connectionId);

        // Act
        await _hub.OnDisconnectedAsync(null);

        // Assert
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Warning) &&
            i.Arguments[2].ToString()!.Contains("User disconnected without valid Id claim"));
    }

    [Test]
    public async Task OnDisconnectedAsync_WhenValid_ShouldRemoveFromGroup()
    {
        // Arrange
        var connectionId = "test-connection-id";
        var userId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new(Claims.Id, userId.ToString()),
            new(Claims.TokenType, TokenTypes.Access)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        
        _contextMock.Setup(x => x.User).Returns(principal);
        _contextMock.Setup(x => x.ConnectionId).Returns(connectionId);
        _groupsMock.Setup(x => x.RemoveFromGroupAsync(connectionId, $"user_{userId}", It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _hub.OnDisconnectedAsync(null);

        // Assert
        _groupsMock.Verify(x => x.RemoveFromGroupAsync(connectionId, $"user_{userId}", It.IsAny<CancellationToken>()), Times.Once);
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Information) &&
            i.Arguments[2].ToString()!.Contains("User") &&
            i.Arguments[2].ToString()!.Contains("disconnected from NotificationHub"));
    }

    [Test]
    public async Task OnDisconnectedAsync_WhenExceptionOccurs_ShouldLogError()
    {
        // Arrange
        var connectionId = "test-connection-id";
        var userId = Guid.NewGuid();
        var exception = new Exception("Test exception");
        var claims = new List<Claim>
        {
            new(Claims.Id, userId.ToString()),
            new(Claims.TokenType, TokenTypes.Access)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        
        _contextMock.Setup(x => x.User).Returns(principal);
        _contextMock.Setup(x => x.ConnectionId).Returns(connectionId);
        _groupsMock.Setup(x => x.RemoveFromGroupAsync(connectionId, $"user_{userId}", It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        await _hub.OnDisconnectedAsync(exception);

        // Assert
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Error) &&
            i.Arguments[2].ToString()!.Contains("Error during disconnect"));
    }
}

