using Application.UseCases.v1.Auth.Command.RequestResetPassword;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Auth.Command.RequestResetPassword;

[TestFixture]
public class RequestResetPasswordValidatorTests
{
    private RequestResetPasswordValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new RequestResetPasswordValidator();
    }

    [Test]
    public void Validate_WhenEmailIsValid_ShouldPass()
    {
        // Arrange
        var command = new RequestResetPasswordCommand { Email = "test@example.com" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenEmailIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new RequestResetPasswordCommand { Email = string.Empty };

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
        var command = new RequestResetPasswordCommand { Email = "invalid-email" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" || e.PropertyName.StartsWith("Email."));
    }
}


