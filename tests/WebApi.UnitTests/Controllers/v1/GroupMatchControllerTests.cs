using Application.UseCases.v1.GroupMatch.Command.RemoveGroupMatchById;
using Application.UseCases.v1.GroupMatch.Query.GetAllGroupMatchesById;
using Application.UseCases.v1.GroupMatch.Query.GetGroupMatchById;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class GroupMatchControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private GroupMatchController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new GroupMatchController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetAllGroupMatchesById_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var response = new GetAllGroupMatchesByIdResponse
        {
            Data = new List<GetAllGroupMatchesByIdResponseData>(),
            Hits = 0
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetAllGroupMatchesByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAllGroupMatchesById(groupId, 1, 10, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task GetGroupMatchById_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var matchId = Guid.NewGuid();
        var response = new GetGroupMatchByIdResponse
        {
            DestinationId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetGroupMatchByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetGroupMatchById(groupId, matchId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task RemoveGroupMatchById_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var groupId = Guid.NewGuid();
        var matchId = Guid.NewGuid();

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<RemoveGroupMatchByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RemoveGroupMatchById(groupId, matchId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
