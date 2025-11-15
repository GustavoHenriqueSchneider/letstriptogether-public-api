using Application.Common.Interfaces.Services;
using Application.UseCases.Group.Query.GetGroupById;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Group.Query.GetGroupById;

[TestFixture]
public class GetGroupByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetGroupByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetGroupByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetGroupByIdResponse()
    {
        // Arrange
        var query = new GetGroupByIdQuery { GroupId = Guid.NewGuid() };
        var expectedResponse = new GetGroupByIdResponse
        {
            Name = "My Group",
            TripExpectedDate = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsCurrentMemberOwner = true,
            Preferences = new GetGroupByIdPreferenceResponse
            {
                LikesShopping = true,
                LikesGastronomy = true,
                Culture = new List<string> { "Museums" },
                Entertainment = new List<string> { "Concerts" },
                PlaceTypes = new List<string> { "Beach" }
            }
        };

        _internalApiServiceMock
            .Setup(x => x.GetGroupByIdAsync(It.IsAny<GetGroupByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetGroupByIdAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
