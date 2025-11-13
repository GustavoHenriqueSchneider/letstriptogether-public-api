using Application.Common.Interfaces.Services;
using Application.UseCases.User.Command.SetCurrentUserPreferences;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.User.Command.SetCurrentUserPreferences;

[TestFixture]
public class SetCurrentUserPreferencesHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private SetCurrentUserPreferencesHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new SetCurrentUserPreferencesHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCallInternalApiService()
    {
        // Arrange
        var command = new SetCurrentUserPreferencesCommand
        {
            LikesCommercial = true,
            Food = new List<string> { "Italian" },
            Culture = new List<string> { "Museums" },
            Entertainment = new List<string> { "Concerts" },
            PlaceTypes = new List<string> { "Beach" }
        };

        _internalApiServiceMock
            .Setup(x => x.SetCurrentUserPreferencesAsync(It.IsAny<SetCurrentUserPreferencesCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _internalApiServiceMock.Verify(
            x => x.SetCurrentUserPreferencesAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
