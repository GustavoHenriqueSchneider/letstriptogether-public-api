using Application.UseCases.v1.GroupMember.Command.RemoveGroupMemberById;
using Application.UseCases.v1.GroupMember.Query.GetGroupMemberById;
using Application.UseCases.v1.GroupMember.Query.GetOtherGroupMembersById;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class GroupMemberControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private GroupMemberController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new GroupMemberController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetOtherGroupMembersById_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var response = new GetOtherGroupMembersByIdResponse
        {
            Data = new List<GetOtherGroupMembersByIdResponseData>(),
            Hits = 0
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetOtherGroupMembersByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetOtherGroupMembersById(groupId, 1, 10, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task GetGroupMemberById_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var response = new GetGroupMemberByIdResponse
        {
            Name = "Test Member",
            IsOwner = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetGroupMemberByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetGroupMemberById(groupId, memberId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task RemoveGroupMemberById_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<RemoveGroupMemberByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RemoveGroupMemberById(groupId, memberId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
