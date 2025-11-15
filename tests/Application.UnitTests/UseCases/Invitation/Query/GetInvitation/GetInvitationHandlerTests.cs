using Application.Common.Interfaces.Services;
using Application.UseCases.Invitation.Query.GetInvitation;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Invitation.Query.GetInvitation;

[TestFixture]
public class GetInvitationHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetInvitationHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetInvitationHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ShouldReturnResponseFromInternalApi()
    {
        // Arrange
        var query = new GetInvitationQuery { Token = "token-value" };
        var expectedResponse = new GetInvitationResponse
        {
            CreatedBy = "Test User",
            GroupName = "Trip Group",
            IsActive = true
        };

        _internalApiServiceMock
            .Setup(x => x.GetInvitationAsync(query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetInvitationAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}



