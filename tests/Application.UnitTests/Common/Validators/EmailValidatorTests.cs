using Application.Common.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Common.Validators;

[TestFixture]
public class EmailValidatorTests
{
    private EmailValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new EmailValidator();
    }

    [Test]
    public void Validate_WhenEmailIsValid_ShouldPass()
    {
        // Arrange
        var email = "test@example.com";

        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenEmailIsEmpty_ShouldFail()
    {
        // Arrange
        var email = string.Empty;

        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("must not be empty"));
    }

    [Test]
    public void Validate_WhenEmailIsNull_ShouldFail()
    {
        // Arrange
        string? email = null;

        // Act & Assert
        Assert.Throws<System.ArgumentNullException>(() => _validator.TestValidate(email!));
    }

    [Test]
    public void Validate_WhenEmailIsInvalidFormat_ShouldFail()
    {
        // Arrange
        var email = "invalid-email";

        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("valid email address"));
    }

    [Test]
    public void Validate_WhenEmailExceedsMaxLength_ShouldFail()
    {
        // Arrange
        var email = new string('a', EmailValidator.EmailMaxLength - 10) + "@example.com";

        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("254") || e.ErrorMessage.Contains("characters"));
    }

    [Test]
    public void Validate_WhenEmailIsAtMaxLength_ShouldPass()
    {
        // Arrange
        var localPart = new string('a', EmailValidator.EmailMaxLength - 12);
        var email = localPart + "@example.com";
        
        Assert.That(email.Length, Is.EqualTo(EmailValidator.EmailMaxLength));

        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    [TestCase("user@domain.com")]
    [TestCase("user.name@domain.com")]
    [TestCase("user+tag@domain.co.uk")]
    [TestCase("user_name@sub-domain.com")]
    public void Validate_WhenEmailHasValidFormats_ShouldPass(string email)
    {
        // Act
        var result = _validator.TestValidate(email);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}


