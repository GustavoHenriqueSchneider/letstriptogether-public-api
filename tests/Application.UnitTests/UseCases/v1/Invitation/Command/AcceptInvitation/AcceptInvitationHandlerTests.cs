using Application.Common.Interfaces.Services;
using Application.UseCases.v1.Invitation.Command.AcceptInvitation;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Invitation.Command.AcceptInvitation;

[TestFixture]
public class AcceptInvitationHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private AcceptInvitationHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new AcceptInvitationHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new AcceptInvitationCommand { Token = "invitation-token" };

        _internalApiServiceMock
            .Setup(x => x.AcceptInvitationAsync(It.IsAny<AcceptInvitationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.AcceptInvitationAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
