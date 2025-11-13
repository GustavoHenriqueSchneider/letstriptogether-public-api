using Application.Common.Interfaces.Services;
using Application.UseCases.Group.Query.GetAllGroups;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Group.Query.GetAllGroups;

[TestFixture]
public class GetAllGroupsHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetAllGroupsHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetAllGroupsHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetAllGroupsResponse()
    {
        // Arrange
        var query = new GetAllGroupsQuery { PageNumber = 1, PageSize = 10 };
        var expectedResponse = new GetAllGroupsResponse
        {
            Data = new List<GetAllGroupsResponseData>
            {
                new GetAllGroupsResponseData { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow }
            },
            Hits = 1
        };

        _internalApiServiceMock
            .Setup(x => x.GetAllGroupsAsync(It.IsAny<GetAllGroupsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetAllGroupsAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
