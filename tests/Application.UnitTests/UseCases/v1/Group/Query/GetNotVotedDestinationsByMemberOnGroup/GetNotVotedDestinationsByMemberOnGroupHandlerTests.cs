using Application.Common.Interfaces.Services;
using Application.UseCases.v1.Group.Query.GetNotVotedDestinationsByMemberOnGroup;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Group.Query.GetNotVotedDestinationsByMemberOnGroup;

[TestFixture]
public class GetNotVotedDestinationsByMemberOnGroupHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetNotVotedDestinationsByMemberOnGroupHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetNotVotedDestinationsByMemberOnGroupHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetNotVotedDestinationsByMemberOnGroupResponse()
    {
        // Arrange
        var query = new GetNotVotedDestinationsByMemberOnGroupQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 10
        };
        var expectedResponse = new GetNotVotedDestinationsByMemberOnGroupResponse
        {
            Data = new List<GetNotVotedDestinationsByMemberOnGroupResponseData>
            {
                new GetNotVotedDestinationsByMemberOnGroupResponseData
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow
                }
            },
            Hits = 1
        };

        _internalApiServiceMock
            .Setup(x => x.GetNotVotedDestinationsByMemberOnGroupAsync(It.IsAny<GetNotVotedDestinationsByMemberOnGroupQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetNotVotedDestinationsByMemberOnGroupAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
