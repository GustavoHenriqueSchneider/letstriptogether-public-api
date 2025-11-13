using Application.Common.Interfaces.Services;
using Application.UseCases.User.Query.GetCurrentUser;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.User.Query.GetCurrentUser;

[TestFixture]
public class GetCurrentUserHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetCurrentUserHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetCurrentUserHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetCurrentUserResponse()
    {
        // Arrange
        var query = new GetCurrentUserQuery();
        var expectedResponse = new GetCurrentUserResponse
        {
            Name = "John Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Preferences = new GetCurrentUserPreferenceResponse
            {
                LikesShopping = true,
                LikesGastronomy = true,
                Culture = new List<string> { "Museums" },
                Entertainment = new List<string> { "Concerts" },
                PlaceTypes = new List<string> { "Beach" }
            }
        };

        _internalApiServiceMock
            .Setup(x => x.GetCurrentUserAsync(It.IsAny<GetCurrentUserQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetCurrentUserAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
