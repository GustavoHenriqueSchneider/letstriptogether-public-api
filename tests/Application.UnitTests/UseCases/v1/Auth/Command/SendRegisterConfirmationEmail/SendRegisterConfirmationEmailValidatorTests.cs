using Application.UseCases.v1.Auth.Command.SendRegisterConfirmationEmail;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Auth.Command.SendRegisterConfirmationEmail;

[TestFixture]
public class SendRegisterConfirmationEmailValidatorTests
{
    private SendRegisterConfirmationEmailValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new SendRegisterConfirmationEmailValidator();
    }

    [Test]
    public void Validate_WhenValidRequest_ShouldPass()
    {
        // Arrange
        var command = new SendRegisterConfirmationEmailCommand
        {
            Name = "John Doe",
            Email = "john@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenNameIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new SendRegisterConfirmationEmailCommand
        {
            Name = string.Empty,
            Email = "john@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    [Test]
    public void Validate_WhenNameExceedsMaxLength_ShouldFail()
    {
        // Arrange
        var command = new SendRegisterConfirmationEmailCommand
        {
            Name = new string('A', SendRegisterConfirmationEmailValidator.NameMaxLength + 1),
            Email = "john@example.com"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validate_WhenEmailIsInvalid_ShouldFail()
    {
        // Arrange
        var command = new SendRegisterConfirmationEmailCommand
        {
            Name = "John Doe",
            Email = "invalid-email"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName == "Email" || e.PropertyName.StartsWith("Email."));
    }
}


