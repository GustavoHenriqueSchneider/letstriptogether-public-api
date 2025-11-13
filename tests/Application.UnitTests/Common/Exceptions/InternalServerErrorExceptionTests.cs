using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions;

[TestFixture]
public class InternalServerErrorExceptionTests
{
    [Test]
    public void Constructor_WhenInternalApiExceptionProvided_ShouldSetProperties()
    {
        // Arrange
        var apiException = new InternalApiException
        {
            Title = "Internal Server Error",
            Status = 500,
            Detail = "An error occurred"
        };

        // Act
        var exception = new InternalServerErrorException(apiException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("An error occurred");
        exception.Title.Should().Be("Internal Server Error");
        exception.StatusCode.Should().Be(500);
    }
}
