using Application.UseCases.Notification.Command.ProcessNotification;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class NotificationControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private NotificationController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new NotificationController(_mediatorMock.Object);
    }

    [Test]
    public async Task ReceiveNotification_WhenValid_ShouldReturnAccepted()
    {
        // Arrange
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = "test_event",
            Data = new { Test = "data" },
            CreatedAt = DateTime.UtcNow
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<ProcessNotificationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ReceiveNotification(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<AcceptedResult>();
        _mediatorMock.Verify(
            x => x.Send(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

