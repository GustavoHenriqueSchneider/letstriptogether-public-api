using Domain.Common;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using WebApi.Hubs;
using WebApi.Services;

namespace WebApi.UnitTests.Services;

[TestFixture]
public class RealTimeNotificationServiceTests
{
    private Mock<IHubContext<NotificationHub>> _hubContextMock = null!;
    private Mock<ILogger<RealTimeNotificationService>> _loggerMock = null!;
    private Mock<IClientProxy> _groupClientProxyMock = null!;
    private RealTimeNotificationService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<RealTimeNotificationService>>();
        _groupClientProxyMock = new Mock<IClientProxy>();
        
        var clientsMock = new Mock<IHubClients>();
        clientsMock.Setup(x => x.Group(It.IsAny<string>())).Returns(_groupClientProxyMock.Object);
        
        _hubContextMock = new Mock<IHubContext<NotificationHub>>();
        _hubContextMock.Setup(x => x.Clients).Returns(clientsMock.Object);
        
        _service = new RealTimeNotificationService(_hubContextMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task SendToUserAsync_WhenUserIdIsEmpty_ShouldLogWarningAndReturn()
    {
        // Arrange
        var payload = new RealTimeNotification
        {
            Type = "test",
            Title = "Test",
            Message = "Test message",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await _service.SendToUserAsync(Guid.Empty, payload, CancellationToken.None);

        // Assert
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Warning) &&
            i.Arguments[2].ToString()!.Contains("Attempted to send notification without user id"));
        
        _hubContextMock.Verify(
            x => x.Clients.Group(It.IsAny<string>()),
            Times.Never);
    }

    [Test]
    public async Task SendToUserAsync_WhenValid_ShouldSendNotificationToGroup()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var payload = new RealTimeNotification
        {
            Type = "match",
            Title = "Test Title",
            Message = "Test Message",
            CreatedAt = DateTime.UtcNow
        };

        _groupClientProxyMock
            .Setup(x => x.SendCoreAsync("ReceiveNotification", It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.SendToUserAsync(userId, payload, CancellationToken.None);

        // Assert
        _groupClientProxyMock.Verify(
            x => x.SendCoreAsync(
                "ReceiveNotification",
                It.IsAny<object[]>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
        
        _hubContextMock.Verify(
            x => x.Clients.Group($"user_{userId}"),
            Times.Once);
    }

    [Test]
    public async Task SendToUserAsync_WhenExceptionOccurs_ShouldLogError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var payload = new RealTimeNotification
        {
            Type = "test",
            Title = "Test",
            Message = "Test message",
            CreatedAt = DateTime.UtcNow
        };

        var exception = new Exception("Test exception");
        _groupClientProxyMock
            .Setup(x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        await _service.SendToUserAsync(userId, payload, CancellationToken.None);

        // Assert
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Error) &&
            i.Arguments[2].ToString()!.Contains("Error while sending realtime notification"));
    }
}

