using Application.Common.Interfaces.Services;
using Application.UseCases.Destination.Query.GetDestinationById;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Destination.Query.GetDestinationById;

[TestFixture]
public class GetDestinationByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetDestinationByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetDestinationByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetDestinationByIdResponse()
    {
        // Arrange
        var query = new GetDestinationByIdQuery { DestinationId = Guid.NewGuid() };
        var expectedResponse = new GetDestinationByIdResponse
        {
            Place = "Test Place",
            Description = "Test Description",
            Attractions = new List<DestinationAttractionModel>(),
            CreatedAt = DateTime.UtcNow
        };

        _internalApiServiceMock
            .Setup(x => x.GetDestinationByIdAsync(It.IsAny<GetDestinationByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetDestinationByIdAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
