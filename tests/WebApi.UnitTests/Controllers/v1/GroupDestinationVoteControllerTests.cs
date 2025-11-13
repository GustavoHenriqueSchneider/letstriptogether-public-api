using Application.UseCases.GroupDestinationVote.Command.UpdateDestinationVoteById;
using Application.UseCases.GroupDestinationVote.Command.VoteAtDestinationForGroupId;
using Application.UseCases.GroupDestinationVote.Query.GetGroupDestinationVoteById;
using Application.UseCases.GroupDestinationVote.Query.GetGroupMemberAllDestinationVotesById;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class GroupDestinationVoteControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private GroupDestinationVoteController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new GroupDestinationVoteController(_mediatorMock.Object);
    }

    [Test]
    public async Task VoteAtDestinationForGroupId_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var command = new VoteAtDestinationForGroupIdCommand
        {
            DestinationId = Guid.NewGuid(),
            IsApproved = true
        };
        var response = new VoteAtDestinationForGroupIdResponse { Id = Guid.NewGuid() };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<VoteAtDestinationForGroupIdCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.VoteAtDestinationForGroupId(groupId, command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task UpdateDestinationVoteById_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var destinationVoteId = Guid.NewGuid();
        var command = new UpdateDestinationVoteByIdCommand { IsApproved = true };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateDestinationVoteByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateDestinationVoteById(groupId, destinationVoteId, command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task GetGroupMemberAllDestinationVotesById_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var response = new GetGroupMemberAllDestinationVotesByIdResponse
        {
            Data = new List<GetGroupMemberAllDestinationVotesByIdResponseData>(),
            Hits = 0
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetGroupMemberAllDestinationVotesByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetGroupMemberAllDestinationVotesById(groupId, 1, 10, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task GetGroupDestinationVoteById_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var destinationVoteId = Guid.NewGuid();
        var response = new GetGroupDestinationVoteByIdResponse
        {
            DestinationId = Guid.NewGuid(),
            IsApproved = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetGroupDestinationVoteByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetGroupDestinationVoteById(groupId, destinationVoteId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }
}
