using Application.Common.Interfaces.Services;
using Application.UseCases.v1.GroupMatch.Command.RemoveGroupMatchById;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupMatch.Command.RemoveGroupMatchById;

[TestFixture]
public class RemoveGroupMatchByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private RemoveGroupMatchByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new RemoveGroupMatchByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new RemoveGroupMatchByIdCommand
        {
            GroupId = Guid.NewGuid(),
            MatchId = Guid.NewGuid()
        };

        _internalApiServiceMock
            .Setup(x => x.RemoveGroupMatchByIdAsync(It.IsAny<RemoveGroupMatchByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.RemoveGroupMatchByIdAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
