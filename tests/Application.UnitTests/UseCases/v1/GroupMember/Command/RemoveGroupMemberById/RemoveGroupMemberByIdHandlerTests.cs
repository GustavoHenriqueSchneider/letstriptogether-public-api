using Application.Common.Interfaces.Services;
using Application.UseCases.v1.GroupMember.Command.RemoveGroupMemberById;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupMember.Command.RemoveGroupMemberById;

[TestFixture]
public class RemoveGroupMemberByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private RemoveGroupMemberByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new RemoveGroupMemberByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new RemoveGroupMemberByIdCommand
        {
            GroupId = Guid.NewGuid(),
            MemberId = Guid.NewGuid()
        };

        _internalApiServiceMock
            .Setup(x => x.RemoveGroupMemberByIdAsync(It.IsAny<RemoveGroupMemberByIdCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.RemoveGroupMemberByIdAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
