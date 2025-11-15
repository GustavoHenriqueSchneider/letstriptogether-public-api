using Application.Common.Interfaces.Services;
using Application.UseCases.v1.GroupInvitation.Query.GetActiveGroupInvitation;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupInvitation.Query.GetActiveGroupInvitation;

[TestFixture]
public class GetActiveGroupInvitationHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetActiveGroupInvitationHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetActiveGroupInvitationHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetActiveGroupInvitationResponse()
    {
        // Arrange
        var query = new GetActiveGroupInvitationQuery { GroupId = Guid.NewGuid() };
        var expectedResponse = new GetActiveGroupInvitationResponse { Token = "active-invitation-token" };

        _internalApiServiceMock
            .Setup(x => x.GetActiveGroupInvitationAsync(It.IsAny<GetActiveGroupInvitationQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetActiveGroupInvitationAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
