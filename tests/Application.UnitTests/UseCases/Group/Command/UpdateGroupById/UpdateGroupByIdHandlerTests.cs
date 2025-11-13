using Application.Common.Interfaces.Services;
using Application.UseCases.Group.Command.UpdateGroupById;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Group.Command.UpdateGroupById;

[TestFixture]
public class UpdateGroupByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private UpdateGroupByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new UpdateGroupByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new UpdateGroupByIdCommand
        {
            GroupId = Guid.NewGuid(),
            Name = "Updated Name",
            TripExpectedDate = DateTime.UtcNow.AddDays(30)
        };

        _internalApiServiceMock
            .Setup(x => x.UpdateGroupByIdAsync(It.IsAny<UpdateGroupByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.UpdateGroupByIdAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
