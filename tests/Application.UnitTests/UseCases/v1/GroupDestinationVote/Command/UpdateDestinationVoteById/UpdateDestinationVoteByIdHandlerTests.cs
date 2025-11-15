using Application.Common.Interfaces.Services;
using Application.UseCases.v1.GroupDestinationVote.Command.UpdateDestinationVoteById;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupDestinationVote.Command.UpdateDestinationVoteById;

[TestFixture]
public class UpdateDestinationVoteByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private UpdateDestinationVoteByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new UpdateDestinationVoteByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new UpdateDestinationVoteByIdCommand
        {
            GroupId = Guid.NewGuid(),
            DestinationVoteId = Guid.NewGuid(),
            IsApproved = true
        };

        _internalApiServiceMock
            .Setup(x => x.UpdateDestinationVoteByIdAsync(It.IsAny<UpdateDestinationVoteByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.UpdateDestinationVoteByIdAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
