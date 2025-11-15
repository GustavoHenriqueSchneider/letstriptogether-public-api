using Application.UseCases.v1.GroupInvitation.Command.CancelActiveGroupInvitation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupInvitation.Command.CancelActiveGroupInvitation;

[TestFixture]
public class CancelActiveGroupInvitationValidatorTests
{
    private CancelActiveGroupInvitationValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new CancelActiveGroupInvitationValidator();
    }

    [Test]
    public void Validate_WhenCommandIsValid_ShouldPass()
    {
        // Arrange
        var command = new CancelActiveGroupInvitationCommand { GroupId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenGroupIdIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new CancelActiveGroupInvitationCommand { GroupId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }
}


