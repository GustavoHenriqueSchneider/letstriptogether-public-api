using Application.Common.Interfaces.Services;
using Application.UseCases.v1.User.Command.AnonymizeCurrentUser;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.User.Command.AnonymizeCurrentUser;

[TestFixture]
public class AnonymizeCurrentUserHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private AnonymizeCurrentUserHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new AnonymizeCurrentUserHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new AnonymizeCurrentUserCommand();

        _internalApiServiceMock
            .Setup(x => x.AnonymizeCurrentUserAsync(It.IsAny<AnonymizeCurrentUserCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.AnonymizeCurrentUserAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
