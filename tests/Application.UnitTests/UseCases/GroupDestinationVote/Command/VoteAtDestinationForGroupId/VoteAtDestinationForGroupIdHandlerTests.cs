using Application.Common.Interfaces.Services;
using Application.UseCases.GroupDestinationVote.Command.VoteAtDestinationForGroupId;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupDestinationVote.Command.VoteAtDestinationForGroupId;

[TestFixture]
public class VoteAtDestinationForGroupIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private VoteAtDestinationForGroupIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new VoteAtDestinationForGroupIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnVoteAtDestinationForGroupIdResponse()
    {
        // Arrange
        var command = new VoteAtDestinationForGroupIdCommand
        {
            GroupId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            IsApproved = true
        };
        var expectedResponse = new VoteAtDestinationForGroupIdResponse { Id = Guid.NewGuid() };

        _internalApiServiceMock
            .Setup(x => x.VoteAtDestinationForGroupIdAsync(It.IsAny<VoteAtDestinationForGroupIdCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.VoteAtDestinationForGroupIdAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
