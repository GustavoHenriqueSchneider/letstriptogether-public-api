using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions;

[TestFixture]
public class BadRequestExceptionTests
{
    [Test]
    public void Constructor_WhenInternalApiExceptionProvided_ShouldSetProperties()
    {
        // Arrange
        var apiException = new InternalApiException
        {
            Title = "Bad Request",
            Status = 400,
            Detail = "Invalid request"
        };

        // Act
        var exception = new BadRequestException(apiException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Invalid request");
        exception.Title.Should().Be("Bad Request");
        exception.StatusCode.Should().Be(400);
    }
}
