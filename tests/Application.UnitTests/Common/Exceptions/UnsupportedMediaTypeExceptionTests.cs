using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions;

[TestFixture]
public class UnsupportedMediaTypeExceptionTests
{
    [Test]
    public void Constructor_WhenInternalApiExceptionProvided_ShouldSetProperties()
    {
        // Arrange
        var apiException = new InternalApiException
        {
            Title = "Unsupported Media Type",
            Status = 415,
            Detail = "Media type not supported"
        };

        // Act
        var exception = new UnsupportedMediaTypeException(apiException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Media type not supported");
        exception.Title.Should().Be("Unsupported Media Type");
        exception.StatusCode.Should().Be(415);
    }
}
