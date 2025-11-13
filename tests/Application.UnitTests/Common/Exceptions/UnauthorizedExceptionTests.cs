using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions;

[TestFixture]
public class UnauthorizedExceptionTests
{
    [Test]
    public void Constructor_WhenInternalApiExceptionProvided_ShouldSetProperties()
    {
        // Arrange
        var apiException = new InternalApiException
        {
            Title = "Unauthorized",
            Status = 401,
            Detail = "Authentication required"
        };

        // Act
        var exception = new UnauthorizedException(apiException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Authentication required");
        exception.Title.Should().Be("Unauthorized");
        exception.StatusCode.Should().Be(401);
    }
}
