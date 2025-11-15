using Application.Common.Interfaces.Services;
using Application.UseCases.v1.Invitation.Command.RefuseInvitation;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Invitation.Command.RefuseInvitation;

[TestFixture]
public class RefuseInvitationHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private RefuseInvitationHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new RefuseInvitationHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new RefuseInvitationCommand { Token = "invitation-token" };

        _internalApiServiceMock
            .Setup(x => x.RefuseInvitationAsync(It.IsAny<RefuseInvitationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.RefuseInvitationAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
