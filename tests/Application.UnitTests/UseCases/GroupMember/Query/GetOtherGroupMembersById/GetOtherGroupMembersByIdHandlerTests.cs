using Application.Common.Interfaces.Services;
using Application.UseCases.GroupMember.Query.GetOtherGroupMembersById;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupMember.Query.GetOtherGroupMembersById;

[TestFixture]
public class GetOtherGroupMembersByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetOtherGroupMembersByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetOtherGroupMembersByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetOtherGroupMembersByIdResponse()
    {
        // Arrange
        var query = new GetOtherGroupMembersByIdQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 10
        };
        var expectedResponse = new GetOtherGroupMembersByIdResponse
        {
            Data = new List<GetOtherGroupMembersByIdResponseData>(),
            Hits = 0
        };

        _internalApiServiceMock
            .Setup(x => x.GetOtherGroupMembersByIdAsync(It.IsAny<GetOtherGroupMembersByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetOtherGroupMembersByIdAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
