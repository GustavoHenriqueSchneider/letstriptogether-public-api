using Application.Common.Interfaces;
using Application.UseCases.Notification.Command.ProcessNotification;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Notification.Command.ProcessNotification;

[TestFixture]
public class ProcessNotificationHandlerTests
{
    private Mock<ILogger<ProcessNotificationHandler>> _loggerMock = null!;
    private ProcessNotificationHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<ProcessNotificationHandler>>();
        _handler = new ProcessNotificationHandler(
            Array.Empty<INotificationEventHandler>(),
            _loggerMock.Object);
    }

    [Test]
    public async Task Handle_WhenHandlerFound_ShouldCallHandler()
    {
        // Arrange
        var eventHandlerMock = new Mock<INotificationEventHandler>();
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = "TestEvent",
            Data = new { Test = "Data" },
            CreatedAt = DateTime.UtcNow
        };

        eventHandlerMock
            .Setup(x => x.CanHandle(command.EventName))
            .Returns(true);

        eventHandlerMock
            .Setup(x => x.HandleAsync(command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handlers = new[] { eventHandlerMock.Object };
        _handler = new ProcessNotificationHandler(handlers, _loggerMock.Object);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        eventHandlerMock.Verify(
            x => x.CanHandle(command.EventName),
            Times.Once);
        eventHandlerMock.Verify(
            x => x.HandleAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenMultipleHandlers_ShouldCallOnlyMatchingHandler()
    {
        // Arrange
        var matchingHandlerMock = new Mock<INotificationEventHandler>();
        var nonMatchingHandlerMock = new Mock<INotificationEventHandler>();
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = "TestEvent",
            Data = new { Test = "Data" },
            CreatedAt = DateTime.UtcNow
        };

        matchingHandlerMock
            .Setup(x => x.CanHandle(command.EventName))
            .Returns(true);

        nonMatchingHandlerMock
            .Setup(x => x.CanHandle(command.EventName))
            .Returns(false);

        matchingHandlerMock
            .Setup(x => x.HandleAsync(command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handlers = new[] { nonMatchingHandlerMock.Object, matchingHandlerMock.Object };
        _handler = new ProcessNotificationHandler(handlers, _loggerMock.Object);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        matchingHandlerMock.Verify(
            x => x.HandleAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
        nonMatchingHandlerMock.Verify(
            x => x.HandleAsync(It.IsAny<ProcessNotificationCommand>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task Handle_WhenNoHandlerFound_ShouldLogWarningAndReturn()
    {
        // Arrange
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = "NonExistentEvent",
            Data = new { Test = "Data" },
            CreatedAt = DateTime.UtcNow
        };

        var handlers = Array.Empty<INotificationEventHandler>();
        _handler = new ProcessNotificationHandler(handlers, _loggerMock.Object);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No handler found")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenHandlerThrowsException_ShouldLogErrorAndRethrow()
    {
        // Arrange
        var eventHandlerMock = new Mock<INotificationEventHandler>();
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = "TestEvent",
            Data = new { Test = "Data" },
            CreatedAt = DateTime.UtcNow
        };
        var expectedException = new Exception("Test exception");

        eventHandlerMock
            .Setup(x => x.CanHandle(command.EventName))
            .Returns(true);

        eventHandlerMock
            .Setup(x => x.HandleAsync(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var handlers = new[] { eventHandlerMock.Object };
        _handler = new ProcessNotificationHandler(handlers, _loggerMock.Object);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>();

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.Is<Exception>(e => e == expectedException),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Test]
    public async Task Handle_ShouldPassCancellationTokenToHandler()
    {
        // Arrange
        var eventHandlerMock = new Mock<INotificationEventHandler>();
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = "TestEvent",
            Data = new { Test = "Data" },
            CreatedAt = DateTime.UtcNow
        };
        var cancellationToken = new CancellationToken(true);

        eventHandlerMock
            .Setup(x => x.CanHandle(command.EventName))
            .Returns(true);

        eventHandlerMock
            .Setup(x => x.HandleAsync(command, cancellationToken))
            .Returns(Task.CompletedTask);

        var handlers = new[] { eventHandlerMock.Object };
        _handler = new ProcessNotificationHandler(handlers, _loggerMock.Object);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        eventHandlerMock.Verify(
            x => x.HandleAsync(command, cancellationToken),
            Times.Once);
    }
}
