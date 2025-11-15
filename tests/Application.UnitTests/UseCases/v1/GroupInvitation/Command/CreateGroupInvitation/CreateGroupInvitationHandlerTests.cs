using Application.Common.Interfaces.Services;
using Application.UseCases.v1.GroupInvitation.Command.CreateGroupInvitation;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupInvitation.Command.CreateGroupInvitation;

[TestFixture]
public class CreateGroupInvitationHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private CreateGroupInvitationHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new CreateGroupInvitationHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnCreateGroupInvitationResponse()
    {
        // Arrange
        var command = new CreateGroupInvitationCommand { GroupId = Guid.NewGuid() };
        var expectedResponse = new CreateGroupInvitationResponse { Token = "invitation-token" };

        _internalApiServiceMock
            .Setup(x => x.CreateGroupInvitationAsync(It.IsAny<CreateGroupInvitationCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.CreateGroupInvitationAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
