using Application.Common.Interfaces.Services;
using Application.UseCases.v1.GroupMatch.Query.GetAllGroupMatchesById;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupMatch.Query.GetAllGroupMatchesById;

[TestFixture]
public class GetAllGroupMatchesByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetAllGroupMatchesByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetAllGroupMatchesByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetAllGroupMatchesByIdResponse()
    {
        // Arrange
        var query = new GetAllGroupMatchesByIdQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 10
        };
        var expectedResponse = new GetAllGroupMatchesByIdResponse
        {
            Data = new List<GetAllGroupMatchesByIdResponseData>(),
            Hits = 0
        };

        _internalApiServiceMock
            .Setup(x => x.GetAllGroupMatchesByIdAsync(It.IsAny<GetAllGroupMatchesByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetAllGroupMatchesByIdAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
