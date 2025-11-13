using Application.UseCases.Auth.Command.Register;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.Register;

[TestFixture]
public class RegisterValidatorTests
{
    private RegisterValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new RegisterValidator();
    }

    [Test]
    public void Validate_WhenValidRequest_ShouldPass()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Password = "ValidPass123!",
            HasAcceptedTermsOfUse = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenPasswordIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Password = string.Empty,
            HasAcceptedTermsOfUse = true
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
        var command = new RegisterCommand
        {
            Password = "weak",
            HasAcceptedTermsOfUse = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" || e.PropertyName.StartsWith("Password."));
    }

    [Test]
    public void Validate_WhenTermsNotAccepted_ShouldFail()
    {
        // Arrange
        var command = new RegisterCommand
        {
            Password = "ValidPass123!",
            HasAcceptedTermsOfUse = false
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HasAcceptedTermsOfUse)
            .WithErrorMessage("Terms of use must be accepted for user registration.");
    }
}


