using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions;

[TestFixture]
public class NotFoundExceptionTests
{
    [Test]
    public void Constructor_WhenInternalApiExceptionProvided_ShouldSetProperties()
    {
        // Arrange
        var apiException = new InternalApiException
        {
            Title = "Not Found",
            Status = 404,
            Detail = "Resource not found"
        };

        // Act
        var exception = new NotFoundException(apiException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Resource not found");
        exception.Title.Should().Be("Not Found");
        exception.StatusCode.Should().Be(404);
    }
}
