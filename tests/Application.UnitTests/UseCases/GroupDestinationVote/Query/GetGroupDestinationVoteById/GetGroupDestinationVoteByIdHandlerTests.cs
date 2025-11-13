using Application.Common.Interfaces.Services;
using Application.UseCases.GroupDestinationVote.Query.GetGroupDestinationVoteById;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupDestinationVote.Query.GetGroupDestinationVoteById;

[TestFixture]
public class GetGroupDestinationVoteByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetGroupDestinationVoteByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetGroupDestinationVoteByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetGroupDestinationVoteByIdResponse()
    {
        // Arrange
        var query = new GetGroupDestinationVoteByIdQuery
        {
            GroupId = Guid.NewGuid(),
            DestinationVoteId = Guid.NewGuid()
        };
        var expectedResponse = new GetGroupDestinationVoteByIdResponse
        {
            DestinationId = Guid.NewGuid(),
            IsApproved = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _internalApiServiceMock
            .Setup(x => x.GetGroupDestinationVoteByIdAsync(It.IsAny<GetGroupDestinationVoteByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetGroupDestinationVoteByIdAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
