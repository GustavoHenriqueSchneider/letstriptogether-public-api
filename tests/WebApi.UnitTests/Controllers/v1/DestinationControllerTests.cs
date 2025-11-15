using Application.UseCases.Destination.Query.GetDestinationById;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers.v1;

namespace WebApi.UnitTests.Controllers.v1;

[TestFixture]
public class DestinationControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private DestinationController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new DestinationController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetDestinationById_WhenValid_ShouldReturnOk()
    {
        // Arrange
        var destinationId = Guid.NewGuid();
        var response = new GetDestinationByIdResponse
        {
            Place = "Test Place",
            Description = "Test Description",
            Image = "https://example.com/image.jpg",
            Attractions = new List<DestinationAttractionModel>(),
            CreatedAt = DateTime.UtcNow
        };

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetDestinationByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetDestinationById(destinationId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }
}
