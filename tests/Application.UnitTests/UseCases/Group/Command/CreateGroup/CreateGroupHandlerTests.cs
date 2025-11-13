using Application.Common.Interfaces.Services;
using Application.UseCases.Group.Command.CreateGroup;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Group.Command.CreateGroup;

[TestFixture]
public class CreateGroupHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private CreateGroupHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new CreateGroupHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnCreateGroupResponse()
    {
        // Arrange
        var command = new CreateGroupCommand
        {
            Name = "My Group",
            TripExpectedDate = DateTime.UtcNow.AddDays(30)
        };
        var expectedResponse = new CreateGroupResponse { Id = Guid.NewGuid() };

        _internalApiServiceMock
            .Setup(x => x.CreateGroupAsync(It.IsAny<CreateGroupCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.CreateGroupAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
