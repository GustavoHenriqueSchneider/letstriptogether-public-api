using Application.UseCases.Invitation.Command.AcceptInvitation;
using Application.UseCases.Invitation.Command.RefuseInvitation;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class InvitationControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private InvitationController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new InvitationController(_mediatorMock.Object);
    }

    [Test]
    public async Task AcceptInvitation_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var command = new AcceptInvitationCommand { Token = "invitation-token" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<AcceptInvitationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AcceptInvitation(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkResult>();
    }

    [Test]
    public async Task RefuseInvitation_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var command = new RefuseInvitationCommand { Token = "invitation-token" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<RefuseInvitationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RefuseInvitation(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
