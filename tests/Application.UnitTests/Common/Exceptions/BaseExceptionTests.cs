using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions;

[TestFixture]
public class BaseExceptionTests
{
    [Test]
    public void Constructor_WhenMessageAndStatusCodeProvided_ShouldSetProperties()
    {
        // Arrange & Act
        var exception = new TestBaseException("Test message", 400, "Test Title");

        // Assert
        exception.Message.Should().Be("Test message");
        exception.StatusCode.Should().Be(400);
        exception.Title.Should().Be("Test Title");
    }

    [Test]
    public void Constructor_WhenTitleIsNull_ShouldSetTitleToNull()
    {
        // Arrange & Act
        var exception = new TestBaseException("Test message", 404, null);

        // Assert
        exception.Message.Should().Be("Test message");
        exception.StatusCode.Should().Be(404);
        exception.Title.Should().BeNull();
    }

    [Test]
    public void Constructor_WhenInternalApiExceptionProvided_ShouldSetPropertiesFromApiException()
    {
        // Arrange
        var apiException = new InternalApiException
        {
            Title = "API Error",
            Status = 500,
            Detail = "API error detail"
        };

        // Act
        var exception = new TestBaseException(apiException);

        // Assert
        exception.Message.Should().Be("API error detail");
        exception.StatusCode.Should().Be(500);
        exception.Title.Should().Be("API Error");
    }

    [Test]
    public void Constructor_WhenInnerExceptionProvided_ShouldSetInnerException()
    {
        // Arrange
        var innerException = new InvalidOperationException("Inner error");

        // Act
        var exception = new TestBaseException("Test message", 400, "Title", innerException);

        // Assert
        exception.InnerException.Should().Be(innerException);
    }

    private class TestBaseException : BaseException
    {
        public TestBaseException(string message, int statusCode, string? title = null, Exception? innerException = null)
            : base(message, statusCode, title, innerException)
        {
        }

        public TestBaseException(InternalApiException apiException)
            : base(apiException)
        {
        }
    }
}
