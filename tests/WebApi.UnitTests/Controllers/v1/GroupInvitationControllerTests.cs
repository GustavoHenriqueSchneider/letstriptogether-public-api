using Application.UseCases.GroupInvitation.Command.CancelActiveGroupInvitation;
using Application.UseCases.GroupInvitation.Command.CreateGroupInvitation;
using Application.UseCases.GroupInvitation.Query.GetActiveGroupInvitation;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class GroupInvitationControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private GroupInvitationController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new GroupInvitationController(_mediatorMock.Object);
    }

    [Test]
    public async Task CreateGroupInvitation_WhenValid_ShouldReturnCreated()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var response = new CreateGroupInvitationResponse { Token = "invitation-token" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreateGroupInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateGroupInvitation(groupId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Test]
    public async Task GetActiveGroupInvitation_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var response = new GetActiveGroupInvitationResponse { Token = "active-token" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetActiveGroupInvitationQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetActiveGroupInvitation(groupId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task CancelActiveGroupInvitation_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var groupId = Guid.NewGuid();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CancelActiveGroupInvitationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CancelActiveGroupInvitation(groupId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
