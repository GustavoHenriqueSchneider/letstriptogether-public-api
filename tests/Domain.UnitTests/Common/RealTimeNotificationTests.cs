using Domain.Common;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.UnitTests.Common;

[TestFixture]
public class RealTimeNotificationTests
{
    [Test]
    public void Constructor_WhenCreated_ShouldInitializeWithDefaultValues()
    {
        // Act
        var notification = new RealTimeNotification();

        // Assert
        notification.Should().NotBeNull();
        notification.Id.Should().NotBeNullOrEmpty();
        notification.Type.Should().Be("match");
        notification.Title.Should().Be(string.Empty);
        notification.Message.Should().Be(string.Empty);
        notification.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void Constructor_WhenCreated_ShouldGenerateUniqueId()
    {
        // Act
        var notification1 = new RealTimeNotification();
        var notification2 = new RealTimeNotification();

        // Assert
        notification1.Id.Should().NotBe(notification2.Id);
    }

    [Test]
    public void Constructor_WhenCreatedWithInitProperties_ShouldSetProperties()
    {
        // Arrange
        var createdAt = DateTime.UtcNow.AddMinutes(-5);

        // Act
        var notification = new RealTimeNotification
        {
            Type = "custom",
            Title = "Test Title",
            Message = "Test Message",
            CreatedAt = createdAt
        };

        // Assert
        notification.Type.Should().Be("custom");
        notification.Title.Should().Be("Test Title");
        notification.Message.Should().Be("Test Message");
        notification.CreatedAt.Should().Be(createdAt);
        notification.Id.Should().NotBeNullOrEmpty();
    }
}

