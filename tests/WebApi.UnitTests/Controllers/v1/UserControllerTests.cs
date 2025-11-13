using Application.UseCases.User.Command.AnonymizeCurrentUser;
using Application.UseCases.User.Command.DeleteCurrentUser;
using Application.UseCases.User.Command.SetCurrentUserPreferences;
using Application.UseCases.User.Command.UpdateCurrentUser;
using Application.UseCases.User.Query.GetCurrentUser;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class UserControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private UserController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UserController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetCurrentUser_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var response = new GetCurrentUserResponse
        {
            Name = "Test User",
            Email = "test@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Preferences = new GetCurrentUserPreferenceResponse
            {
                LikesShopping = true,
                LikesGastronomy = true,
                Culture = new List<string>(),
                Entertainment = new List<string>(),
                PlaceTypes = new List<string>()
            }
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetCurrentUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetCurrentUser(CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test]
    public async Task UpdateCurrentUser_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var command = new UpdateCurrentUserCommand { Name = "Updated Name" };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateCurrentUserCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateCurrentUser(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task DeleteCurrentUser_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<DeleteCurrentUserCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteCurrentUser(CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task AnonymizeCurrentUser_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<AnonymizeCurrentUserCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AnonymizeCurrentUser(CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task SetCurrentUserPreferences_WhenValid_ShouldReturnNoContent()
    {
        // Arrange
        var command = new SetCurrentUserPreferencesCommand
        {
            LikesShopping = true,
            LikesGastronomy = true,
            Culture = new List<string>(),
            Entertainment = new List<string>(),
            PlaceTypes = new List<string>()
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<SetCurrentUserPreferencesCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.SetCurrentUserPreferences(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}
