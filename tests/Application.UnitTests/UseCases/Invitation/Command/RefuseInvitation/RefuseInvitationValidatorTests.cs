using Application.UseCases.Invitation.Command.RefuseInvitation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Invitation.Command.RefuseInvitation;

[TestFixture]
public class RefuseInvitationValidatorTests
{
    private RefuseInvitationValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new RefuseInvitationValidator();
    }

    [Test]
    public void Validate_WhenCommandIsValid_ShouldPass()
    {
        // Arrange
        var command = new RefuseInvitationCommand { Token = "valid-token" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenTokenIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new RefuseInvitationCommand { Token = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Token)
            .WithErrorMessage("'Token' must not be empty.");
    }

    [Test]
    public void Validate_WhenTokenIsNull_ShouldFail()
    {
        // Arrange
        var command = new RefuseInvitationCommand { Token = null! };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Token)
            .WithErrorMessage("'Token' must not be empty.");
    }
}


