using Application.UseCases.Group.Command.CreateGroup;
using Application.UseCases.Group.Command.DeleteGroupById;
using Application.UseCases.Group.Command.LeaveGroupById;
using Application.UseCases.Group.Command.UpdateGroupById;
using Application.UseCases.Group.Query.GetAllGroups;
using Application.UseCases.Group.Query.GetGroupById;
using Application.UseCases.Group.Query.GetNotVotedDestinationsByMemberOnGroup;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class GroupControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private GroupController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new GroupController(_mediatorMock.Object);
    }

    [Test]
    public async Task CreateGroup_WhenValid_ShouldReturnCreated()
    {
        // Arrange
        var command = new CreateGroupCommand { Name = "Test Group", TripExpectedDate = DateTime.UtcNow.AddDays(30) };
        var response = new CreateGroupResponse { Id = Guid.NewGuid() };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreateGroupCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateGroup(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Test]
    public async Task GetAllGroups_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var response = new GetAllGroupsResponse { Data = new List<GetAllGroupsResponseData>(), Hits = 0 };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetAllGroupsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAllGroups(1, 10, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task GetGroupById_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var response = new GetGroupByIdResponse
        {
            Name = "Test Group",
            TripExpectedDate = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsCurrentMemberOwner = true,
            Preferences = new GetGroupByIdPreferenceResponse
            {
                LikesShopping = true,
                LikesGastronomy = true,
                Culture = new List<string>(),
                Entertainment = new List<string>(),
                PlaceTypes = new List<string>()
            }
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetGroupByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetGroupById(groupId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task UpdateGroupById_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var command = new UpdateGroupByIdCommand { Name = "Updated Group" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateGroupByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateGroupById(groupId, command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task DeleteGroupById_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var groupId = Guid.NewGuid();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<DeleteGroupByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteGroupById(groupId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task LeaveGroupById_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var groupId = Guid.NewGuid();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<LeaveGroupByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.LeaveGroupById(groupId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task GetNotVotedDestinationsByMemberOnGroup_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var response = new GetNotVotedDestinationsByMemberOnGroupResponse
        {
            Data = new List<GetNotVotedDestinationsByMemberOnGroupResponseData>(),
            Hits = 0
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetNotVotedDestinationsByMemberOnGroupQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetNotVotedDestinationsByMemberOnGroup(groupId, 1, 10, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }
}
