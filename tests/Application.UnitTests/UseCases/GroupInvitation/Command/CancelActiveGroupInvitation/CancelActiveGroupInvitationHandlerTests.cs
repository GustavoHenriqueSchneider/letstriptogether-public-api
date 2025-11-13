using Application.Common.Interfaces.Services;
using Application.UseCases.GroupInvitation.Command.CancelActiveGroupInvitation;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupInvitation.Command.CancelActiveGroupInvitation;

[TestFixture]
public class CancelActiveGroupInvitationHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private CancelActiveGroupInvitationHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new CancelActiveGroupInvitationHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new CancelActiveGroupInvitationCommand { GroupId = Guid.NewGuid() };

        _internalApiServiceMock
            .Setup(x => x.CancelActiveGroupInvitationAsync(It.IsAny<CancelActiveGroupInvitationCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.CancelActiveGroupInvitationAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
