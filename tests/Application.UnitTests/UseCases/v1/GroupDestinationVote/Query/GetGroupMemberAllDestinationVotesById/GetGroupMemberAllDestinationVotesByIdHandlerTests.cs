using Application.Common.Interfaces.Services;
using Application.UseCases.v1.GroupDestinationVote.Query.GetGroupMemberAllDestinationVotesById;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupDestinationVote.Query.GetGroupMemberAllDestinationVotesById;

[TestFixture]
public class GetGroupMemberAllDestinationVotesByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetGroupMemberAllDestinationVotesByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetGroupMemberAllDestinationVotesByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetGroupMemberAllDestinationVotesByIdResponse()
    {
        // Arrange
        var query = new GetGroupMemberAllDestinationVotesByIdQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 10
        };
        var expectedResponse = new GetGroupMemberAllDestinationVotesByIdResponse
        {
            Data = new List<GetGroupMemberAllDestinationVotesByIdResponseData>(),
            Hits = 0
        };

        _internalApiServiceMock
            .Setup(x => x.GetGroupMemberAllDestinationVotesByIdAsync(It.IsAny<GetGroupMemberAllDestinationVotesByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetGroupMemberAllDestinationVotesByIdAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
