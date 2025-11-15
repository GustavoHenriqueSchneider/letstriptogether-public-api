using Application.Common.Interfaces.Services;
using Application.UseCases.User.Command.ChangeCurrentUserPassword;
using FluentAssertions;
using Infrastructure.Services;
using Moq;
using NUnit.Framework;

namespace Infrastructure.UnitTests.Services;

[TestFixture]
public class InternalApiServiceChangePasswordTests
{
    private Mock<IHttpClientService> _httpClientServiceMock = null!;
    private InternalApiService _internalApiService = null!;

    [SetUp]
    public void SetUp()
    {
        _httpClientServiceMock = new Mock<IHttpClientService>();
        _internalApiService = new InternalApiService(_httpClientServiceMock.Object);
    }

    [Test]
    public async Task ChangeCurrentUserPasswordAsync_ShouldCallHttpClientServiceWithCorrectUrl()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "currentPassword",
            NewPassword = "newPassword"
        };

        _httpClientServiceMock
            .Setup(x => x.PostAsync("v1/users/me/change-password", command, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _internalApiService.ChangeCurrentUserPasswordAsync(command, CancellationToken.None);

        // Assert
        _httpClientServiceMock.Verify(
            x => x.PostAsync("v1/users/me/change-password", command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}

