using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions;

[TestFixture]
public class ForbiddenExceptionTests
{
    [Test]
    public void Constructor_WhenInternalApiExceptionProvided_ShouldSetProperties()
    {
        // Arrange
        var apiException = new InternalApiException
        {
            Title = "Forbidden",
            Status = 403,
            Detail = "Access denied"
        };

        // Act
        var exception = new ForbiddenException(apiException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Access denied");
        exception.Title.Should().Be("Forbidden");
        exception.StatusCode.Should().Be(403);
    }
}
