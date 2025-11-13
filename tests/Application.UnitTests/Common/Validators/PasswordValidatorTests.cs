using Application.Common.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Common.Validators;

[TestFixture]
public class PasswordValidatorTests
{
    private PasswordValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new PasswordValidator();
    }

    [Test]
    public void Validate_WhenPasswordIsValid_ShouldPass()
    {
        // Arrange
        var password = "ValidPass123!";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenPasswordIsEmpty_ShouldFail()
    {
        // Arrange
        var password = string.Empty;

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("must not be empty"));
    }

    [Test]
    public void Validate_WhenPasswordIsNull_ShouldFail()
    {
        // Arrange
        string? password = null;

        // Act & Assert
        Assert.Throws<System.ArgumentNullException>(() => _validator.TestValidate(password!));
    }

    [Test]
    public void Validate_WhenPasswordIsTooShort_ShouldFail()
    {
        // Arrange
        var password = "Short1!";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("must be at least") || e.ErrorMessage.Contains($"{PasswordValidator.PasswordMinLength}"));
    }

    [Test]
    public void Validate_WhenPasswordExceedsMaxLength_ShouldFail()
    {
        // Arrange
        var password = new string('A', PasswordValidator.PasswordMaxLength + 1) + "1a!";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("must be less than") || e.ErrorMessage.Contains($"{PasswordValidator.PasswordMaxLength}"));
    }

    [Test]
    public void Validate_WhenPasswordHasNoUppercase_ShouldFail()
    {
        // Arrange
        var password = "lowercase123!";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("uppercase"));
    }

    [Test]
    public void Validate_WhenPasswordHasNoLowercase_ShouldFail()
    {
        // Arrange
        var password = "UPPERCASE123!";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("lowercase"));
    }

    [Test]
    public void Validate_WhenPasswordHasNoNumber_ShouldFail()
    {
        // Arrange
        var password = "NoNumberHere!";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("number"));
    }

    [Test]
    public void Validate_WhenPasswordHasNoSpecialCharacter_ShouldFail()
    {
        // Arrange
        var password = "NoSpecialChar123";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("special"));
    }

    [Test]
    public void Validate_WhenPasswordIsAtMinLength_ShouldPass()
    {
        // Arrange
        var password = "Valid1!@";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenPasswordIsAtMaxLength_ShouldPass()
    {
        // Arrange
        var password = new string('A', PasswordValidator.PasswordMaxLength - 3) + "1a!";

        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    [TestCase("Password1!")]
    [TestCase("Secure123@")]
    [TestCase("MyPass456#")]
    [TestCase("TestPass789$")]
    public void Validate_WhenPasswordMeetsAllRequirements_ShouldPass(string password)
    {
        // Act
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}


