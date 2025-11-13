using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions;

[TestFixture]
public class ConflictExceptionTests
{
    [Test]
    public void Constructor_WhenInternalApiExceptionProvided_ShouldSetProperties()
    {
        // Arrange
        var apiException = new InternalApiException
        {
            Title = "Conflict",
            Status = 409,
            Detail = "Resource conflict"
        };

        // Act
        var exception = new ConflictException(apiException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Resource conflict");
        exception.Title.Should().Be("Conflict");
        exception.StatusCode.Should().Be(409);
    }
}
