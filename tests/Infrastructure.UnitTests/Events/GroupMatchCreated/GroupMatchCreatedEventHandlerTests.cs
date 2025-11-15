using System.Text.Json;
using Application.Common.Interfaces.Services;
using Application.UseCases.v1.Destination.Query.GetDestinationById;
using Application.UseCases.v1.Group.Query.GetGroupById;
using Application.UseCases.v1.Notification.Command.ProcessNotification;
using Domain.Events;
using FluentAssertions;
using Infrastructure.Events.GroupMatchCreated;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Infrastructure.UnitTests.Events.GroupMatchCreated;

[TestFixture]
public class GroupMatchCreatedEventHandlerTests
{
    private Mock<ILogger<GroupMatchCreatedEventHandler>> _loggerMock = null!;
    private Mock<IRealTimeNotificationService> _realTimeNotificationServiceMock = null!;
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GroupMatchCreatedEventHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<GroupMatchCreatedEventHandler>>();
        _realTimeNotificationServiceMock = new Mock<IRealTimeNotificationService>();
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GroupMatchCreatedEventHandler(
            _loggerMock.Object,
            _realTimeNotificationServiceMock.Object,
            _internalApiServiceMock.Object);
    }

    [Test]
    public void CanHandle_WhenEventNameIsGroupMatchCreated_ShouldReturnTrue()
    {
        // Act
        var result = _handler.CanHandle(NotificationEvents.GroupMatchCreated);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void CanHandle_WhenEventNameIsNotGroupMatchCreated_ShouldReturnFalse()
    {
        // Act
        var result = _handler.CanHandle("other_event");

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task HandleAsync_WhenEventDataIsNull_ShouldLogWarningAndReturn()
    {
        // Arrange
        // Use a JSON array which cannot be deserialized to GroupMatchCreatedEventData (object)
        var invalidJson = JsonSerializer.SerializeToElement(new[] { 1, 2, 3 }, new JsonSerializerOptions());
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = NotificationEvents.GroupMatchCreated,
            Data = invalidJson,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Warning) &&
            i.Arguments[2].ToString()!.Contains("GroupMatchCreated event data could not be deserialized"));
        _realTimeNotificationServiceMock.Verify(
            x => x.SendToUserAsync(It.IsAny<Guid>(), It.IsAny<Domain.Common.RealTimeNotification>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task HandleAsync_WhenUserIdIsEmpty_ShouldLogWarningAndReturn()
    {
        // Arrange
        var eventData = new GroupMatchCreatedEventData
        {
            GroupId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid()
        };
        var jsonElement = JsonSerializer.SerializeToElement(eventData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.Empty,
            EventName = NotificationEvents.GroupMatchCreated,
            Data = jsonElement,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Warning) &&
            i.Arguments[2].ToString()!.Contains("GroupMatchCreated event received without user id"));
        _realTimeNotificationServiceMock.Verify(
            x => x.SendToUserAsync(It.IsAny<Guid>(), It.IsAny<Domain.Common.RealTimeNotification>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task HandleAsync_WhenGroupNotFound_ShouldLogWarningAndReturn()
    {
        // Arrange
        var eventData = new GroupMatchCreatedEventData
        {
            GroupId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid()
        };
        var jsonElement = JsonSerializer.SerializeToElement(eventData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = NotificationEvents.GroupMatchCreated,
            Data = jsonElement,
            CreatedAt = DateTime.UtcNow
        };

        _internalApiServiceMock
            .Setup(x => x.GetGroupByIdAsync(It.IsAny<GetGroupByIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Group not found"));

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Warning) &&
            i.Arguments[2].ToString()!.Contains("Group not found for GroupMatchCreated event"));
        _realTimeNotificationServiceMock.Verify(
            x => x.SendToUserAsync(It.IsAny<Guid>(), It.IsAny<Domain.Common.RealTimeNotification>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task HandleAsync_WhenDestinationNotFound_ShouldLogWarningAndReturn()
    {
        // Arrange
        var eventData = new GroupMatchCreatedEventData
        {
            GroupId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid()
        };
        var jsonElement = JsonSerializer.SerializeToElement(eventData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        var userId = Guid.NewGuid();
        var command = new ProcessNotificationCommand
        {
            UserId = userId,
            EventName = NotificationEvents.GroupMatchCreated,
            Data = jsonElement,
            CreatedAt = DateTime.UtcNow
        };

        var group = new GetGroupByIdResponse
        {
            Name = "Test Group",
            TripExpectedDate = DateTime.UtcNow.AddDays(30),
            IsCurrentMemberOwner = false,
            Preferences = new GetGroupByIdPreferenceResponse
            {
                LikesShopping = false,
                LikesGastronomy = false,
                Culture = new List<string>(),
                Entertainment = new List<string>(),
                PlaceTypes = new List<string>()
            },
            CreatedAt = DateTime.UtcNow
        };

        _internalApiServiceMock
            .Setup(x => x.GetGroupByIdAsync(It.IsAny<GetGroupByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);

        _internalApiServiceMock
            .Setup(x => x.GetDestinationByIdAsync(It.IsAny<GetDestinationByIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Destination not found"));

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        _loggerMock.Invocations.Should().Contain(i =>
            i.Method.Name == "Log" &&
            Equals(i.Arguments[0], LogLevel.Warning) &&
            i.Arguments[2].ToString()!.Contains("Destination not found for GroupMatchCreated event"));
        _realTimeNotificationServiceMock.Verify(
            x => x.SendToUserAsync(It.IsAny<Guid>(), It.IsAny<Domain.Common.RealTimeNotification>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task HandleAsync_WhenValid_ShouldSendNotification()
    {
        // Arrange
        var eventData = new GroupMatchCreatedEventData
        {
            GroupId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid()
        };
        var jsonElement = JsonSerializer.SerializeToElement(eventData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        var userId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var command = new ProcessNotificationCommand
        {
            UserId = userId,
            EventName = NotificationEvents.GroupMatchCreated,
            Data = jsonElement,
            CreatedAt = createdAt
        };

        var group = new GetGroupByIdResponse
        {
            Name = "Test Group",
            TripExpectedDate = DateTime.UtcNow.AddDays(30),
            IsCurrentMemberOwner = false,
            Preferences = new GetGroupByIdPreferenceResponse
            {
                LikesShopping = false,
                LikesGastronomy = false,
                Culture = new List<string>(),
                Entertainment = new List<string>(),
                PlaceTypes = new List<string>()
            },
            CreatedAt = DateTime.UtcNow
        };

        var destination = new GetDestinationByIdResponse
        {
            Place = "Test Place",
            Description = "Test Description",
            Image = "https://example.com/image.jpg",
            Attractions = new List<DestinationAttractionModel>(),
            CreatedAt = DateTime.UtcNow
        };

        _internalApiServiceMock
            .Setup(x => x.GetGroupByIdAsync(It.IsAny<GetGroupByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(group);

        _internalApiServiceMock
            .Setup(x => x.GetDestinationByIdAsync(It.IsAny<GetDestinationByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(destination);

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        _realTimeNotificationServiceMock.Verify(
            x => x.SendToUserAsync(
                userId,
                It.Is<Domain.Common.RealTimeNotification>(n =>
                    n.Type == "match" &&
                    n.Title == "Novo match!" &&
                    n.Message.Contains("Test Group") &&
                    n.Message.Contains("Test Place") &&
                    n.CreatedAt == createdAt),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

