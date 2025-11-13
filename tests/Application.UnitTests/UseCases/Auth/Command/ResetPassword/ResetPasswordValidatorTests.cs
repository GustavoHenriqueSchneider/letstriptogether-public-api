using Application.UseCases.Auth.Command.ResetPassword;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.ResetPassword;

[TestFixture]
public class ResetPasswordValidatorTests
{
    private ResetPasswordValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new ResetPasswordValidator();
    }

    [Test]
    public void Validate_WhenPasswordIsValid_ShouldPass()
    {
        // Arrange
        var command = new ResetPasswordCommand { Password = "ValidPass123!" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenPasswordIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new ResetPasswordCommand { Password = string.Empty };

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
        var command = new ResetPasswordCommand { Password = "weak" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" || e.PropertyName.StartsWith("Password."));
    }
}


