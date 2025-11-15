using Application.UseCases.v1.Auth.Command.Login;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Auth.Command.Login;

[TestFixture]
public class LoginValidatorTests
{
    private LoginValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new LoginValidator();
    }

    [Test]
    public void Validate_WhenValidRequest_ShouldPass()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "ValidPass123!"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenEmailIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = string.Empty,
            Password = "ValidPass123!"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" || e.PropertyName.StartsWith("Email."));
    }

    [Test]
    public void Validate_WhenEmailIsInvalid_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "invalid-email",
            Password = "ValidPass123!"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" || e.PropertyName.StartsWith("Email."));
    }

    [Test]
    public void Validate_WhenPasswordIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" || e.PropertyName.StartsWith("Password."));
    }

    [Test]
    public void Validate_WhenPasswordIsInvalid_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = "weak"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" || e.PropertyName.StartsWith("Password."));
    }

    [Test]
    public void Validate_WhenBothEmailAndPasswordAreEmpty_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = string.Empty,
            Password = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" || e.PropertyName.StartsWith("Email."));
        result.Errors.Should().Contain(e => e.PropertyName == "Password" || e.PropertyName.StartsWith("Password."));
    }
}
