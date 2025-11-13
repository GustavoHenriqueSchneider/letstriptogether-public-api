using Application.Common.Interfaces.Services;
using Application.UseCases.GroupMember.Query.GetGroupMemberById;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupMember.Query.GetGroupMemberById;

[TestFixture]
public class GetGroupMemberByIdHandlerTests
{
    private Mock<IInternalApiService> _internalApiServiceMock = null!;
    private GetGroupMemberByIdHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _internalApiServiceMock = new Mock<IInternalApiService>();
        _handler = new GetGroupMemberByIdHandler(_internalApiServiceMock.Object);
    }

    [Test]
    public async Task Handle_WhenValidRequest_ShouldReturnGetGroupMemberByIdResponse()
    {
        // Arrange
        var query = new GetGroupMemberByIdQuery
        {
            GroupId = Guid.NewGuid(),
            MemberId = Guid.NewGuid()
        };
        var expectedResponse = new GetGroupMemberByIdResponse
        {
            Name = "Test Member",
            IsOwner = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _internalApiServiceMock
            .Setup(x => x.GetGroupMemberByIdAsync(It.IsAny<GetGroupMemberByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _internalApiServiceMock.Verify(
            x => x.GetGroupMemberByIdAsync(query, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
