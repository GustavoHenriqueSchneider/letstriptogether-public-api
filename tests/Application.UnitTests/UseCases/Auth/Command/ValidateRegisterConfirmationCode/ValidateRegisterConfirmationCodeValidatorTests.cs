using Application.UseCases.Auth.Command.ValidateRegisterConfirmationCode;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.ValidateRegisterConfirmationCode;

[TestFixture]
public class ValidateRegisterConfirmationCodeValidatorTests
{
    private ValidateRegisterConfirmationCodeValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new ValidateRegisterConfirmationCodeValidator();
    }

    [Test]
    public void Validate_WhenCodeIsValid_ShouldPass()
    {
        // Arrange
        var command = new ValidateRegisterConfirmationCodeCommand { Code = 123456 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenCodeIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new ValidateRegisterConfirmationCodeCommand { Code = 0 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code)
            .WithErrorMessage("'Code' must not be empty.");
    }

    [Test]
    public void Validate_WhenCodeIsLessThan100000_ShouldFail()
    {
        // Arrange
        var command = new ValidateRegisterConfirmationCodeCommand { Code = 99999 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code)
            .WithErrorMessage("'Code' must be greater than or equal to '100000'.");
    }

    [Test]
    public void Validate_WhenCodeIsGreaterThan999999_ShouldFail()
    {
        // Arrange
        var command = new ValidateRegisterConfirmationCodeCommand { Code = 1000000 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Code)
            .WithErrorMessage("'Code' must be less than or equal to '999999'.");
    }

    [Test]
    public void Validate_WhenCodeIsExactly100000_ShouldPass()
    {
        // Arrange
        var command = new ValidateRegisterConfirmationCodeCommand { Code = 100000 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenCodeIsExactly999999_ShouldPass()
    {
        // Arrange
        var command = new ValidateRegisterConfirmationCodeCommand { Code = 999999 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}


