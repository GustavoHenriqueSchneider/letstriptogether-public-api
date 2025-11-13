using Application.Common.Interfaces.Services;
using Application.UseCases.GroupMatch.Query.GetGroupMatchById;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupMatch.Query.GetGroupMatchById;

[TestFixture]
public class GetGroupMatchByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetGroupMatchByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetGroupMatchByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetGroupMatchByIdResponse()
    {
        // Arrange
        var query = new GetGroupMatchByIdQuery
        {
            GroupId = Guid.NewGuid(),
            MatchId = Guid.NewGuid()
        };
        var expectedResponse = new GetGroupMatchByIdResponse
        {
            DestinationId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _internalApiServiceMock
            .Setup(x => x.GetGroupMatchByIdAsync(It.IsAny<GetGroupMatchByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetGroupMatchByIdAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
