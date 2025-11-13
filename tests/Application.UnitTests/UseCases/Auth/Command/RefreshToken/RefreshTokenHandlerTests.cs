using Application.Common.Interfaces.Services;
using Application.UseCases.Auth.Command.RefreshToken;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.RefreshToken;

[TestFixture]
public class RefreshTokenHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private RefreshTokenHandler _refreshTokenHandler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _refreshTokenHandler = new RefreshTokenHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnRefreshTokenResponse()
    {
        // Arrange
        var command = new RefreshTokenCommand { RefreshToken = "refresh-token-123" };
        var expectedResponse = new RefreshTokenResponse
        {
            AccessToken = "new-access-token",
            RefreshToken = "new-refresh-token"
        };

        _internalApiServiceMock
            .Setup(x => x.RefreshTokenAsync(It.IsAny<RefreshTokenCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _refreshTokenHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.RefreshTokenAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
