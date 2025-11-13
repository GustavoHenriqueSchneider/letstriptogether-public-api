using Domain.Common.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Domain.UnitTests.Common.Exceptions;

[TestFixture]
public class DomainBusinessRuleExceptionTests
{
    [Test]
    public void Constructor_WhenInternalApiExceptionProvided_ShouldSetProperties()
    {
        // Arrange
        var internalApiException = new InternalApiException
        {
            Title = "Business Rule Violation",
            Status = 422,
            Detail = "Business rule violated"
        };

        // Act
        var exception = new DomainBusinessRuleException(internalApiException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Business rule violated");
        exception.Title.Should().Be("Business Rule Violation");
        exception.StatusCode.Should().Be(422);
    }

    [Test]
    public void Constructor_WhenInternalApiExceptionHasNullTitle_ShouldSetTitle()
    {
        // Arrange
        var internalApiException = new InternalApiException
        {
            Title = null!,
            Status = 422,
            Detail = "Business rule violated"
        };

        // Act
        var exception = new DomainBusinessRuleException(internalApiException);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Be("Business rule violated");
        exception.Title.Should().BeNull();
        exception.StatusCode.Should().Be(422);
    }

    [Test]
    public void StatusCode_ShouldAlwaysBe422()
    {
        // Arrange
        var internalApiException = new InternalApiException
        {
            Title = "Test",
            Status = 500,
            Detail = "Test"
        };

        // Act
        var exception = new DomainBusinessRuleException(internalApiException);

        // Assert
        exception.StatusCode.Should().Be(422);
    }
}
