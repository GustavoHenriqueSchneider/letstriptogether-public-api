using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.UnitTests.Common.Exceptions;

[TestFixture]
public class InternalApiExceptionTests
{
    [Test]
    public void Constructor_WhenAllPropertiesSet_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var exception = new InternalApiException
        {
            Title = "Test Title",
            Status = 400,
            Detail = "Test Detail"
        };

        // Assert
        exception.Should().NotBeNull();
        exception.Title.Should().Be("Test Title");
        exception.Status.Should().Be(400);
        exception.Detail.Should().Be("Test Detail");
    }

    [Test]
    public void Properties_ShouldBeMutable()
    {
        // Arrange
        var exception = new InternalApiException
        {
            Title = "Initial Title",
            Status = 400,
            Detail = "Initial Detail"
        };

        // Act
        var newException = new InternalApiException
        {
            Title = "New Title",
            Status = 500,
            Detail = "New Detail"
        };

        // Assert
        newException.Title.Should().Be("New Title");
        newException.Status.Should().Be(500);
        newException.Detail.Should().Be("New Detail");
    }
}
