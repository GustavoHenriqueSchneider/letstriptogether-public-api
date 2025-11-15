using Application.UseCases.v1.User.Command.ChangeCurrentUserPassword;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.User.Command.ChangeCurrentUserPassword;

[TestFixture]
public class ChangeCurrentUserPasswordValidatorTests
{
    private ChangeCurrentUserPasswordValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new ChangeCurrentUserPasswordValidator();
    }

    [Test]
    public void Validate_WhenValidRequest_ShouldPass()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "CurrentPass123!",
            NewPassword = "NewPass123!"
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenCurrentPasswordIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = string.Empty,
            NewPassword = "NewPass123!"
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("CurrentPassword.Password");
    }

    [Test]
    public void Validate_WhenCurrentPasswordIsNull_ShouldFail()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = null!,
            NewPassword = "NewPass123!"
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CurrentPassword);
    }

    [Test]
    public void Validate_WhenCurrentPasswordIsInvalid_ShouldFail()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "weak",
            NewPassword = "NewPass123!"
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("CurrentPassword.Password");
    }

    [Test]
    public void Validate_WhenNewPasswordIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "CurrentPass123!",
            NewPassword = string.Empty
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("NewPassword.Password");
    }

    [Test]
    public void Validate_WhenNewPasswordIsNull_ShouldFail()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "CurrentPass123!",
            NewPassword = null!
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Test]
    public void Validate_WhenNewPasswordIsInvalid_ShouldFail()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "CurrentPass123!",
            NewPassword = "weak"
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("NewPassword.Password");
    }

    [Test]
    public void Validate_WhenBothPasswordsAreEmpty_ShouldFail()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = string.Empty,
            NewPassword = string.Empty
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("CurrentPassword.Password");
        result.ShouldHaveValidationErrorFor("NewPassword.Password");
    }

    [Test]
    public void Validate_WhenBothPasswordsAreInvalid_ShouldFail()
    {
        // Arrange
        var command = new ChangeCurrentUserPasswordCommand
        {
            CurrentPassword = "weak1",
            NewPassword = "weak2"
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("CurrentPassword.Password");
        result.ShouldHaveValidationErrorFor("NewPassword.Password");
    }
}
