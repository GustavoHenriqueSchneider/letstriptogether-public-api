using Application.Common.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace Application.UnitTests.Common.Exceptions;

[TestFixture]
public class ValidationExceptionTests
{
    [Test]
    public void Constructor_WhenErrorsProvided_ShouldSetProperties()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Email", new[] { "Email is required" } },
            { "Password", new[] { "Password is too short" } }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        exception.Message.Should().Be("One or more validation errors occurred.");
        exception.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        exception.Title.Should().Be("Validation Error");
        exception.Errors.Should().BeEquivalentTo(errors);
        exception.Errors.Should().HaveCount(2);
    }

    [Test]
    public void Constructor_WhenErrorsIsEmpty_ShouldSetProperties()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>();

        // Act
        var exception = new ValidationException(errors);

        // Assert
        exception.Message.Should().Be("One or more validation errors occurred.");
        exception.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        exception.Title.Should().Be("Validation Error");
        exception.Errors.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WhenMultipleErrorsForSameProperty_ShouldGroupErrors()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Email", new[] { "Email is required", "Email format is invalid" } },
            { "Password", new[] { "Password is required" } }
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        exception.Errors.Should().HaveCount(2);
        exception.Errors["Email"].Should().HaveCount(2);
        exception.Errors["Email"].Should().Contain("Email is required");
        exception.Errors["Email"].Should().Contain("Email format is invalid");
        exception.Errors["Password"].Should().HaveCount(1);
    }

    [Test]
    public void Constructor_WhenInnerExceptionProvided_ShouldSetInnerException()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Field", new[] { "Error" } }
        };
        var innerException = new InvalidOperationException("Inner error");

        // Act
        var exception = new ValidationException(errors, innerException);

        // Assert
        exception.InnerException.Should().Be(innerException);
    }
}
