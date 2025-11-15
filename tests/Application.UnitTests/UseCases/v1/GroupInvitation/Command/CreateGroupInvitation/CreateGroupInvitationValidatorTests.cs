using Application.UseCases.v1.GroupInvitation.Command.CreateGroupInvitation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupInvitation.Command.CreateGroupInvitation;

[TestFixture]
public class CreateGroupInvitationValidatorTests
{
    private CreateGroupInvitationValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateGroupInvitationValidator();
    }

    [Test]
    public void Validate_WhenCommandIsValid_ShouldPass()
    {
        // Arrange
        var command = new CreateGroupInvitationCommand { GroupId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenGroupIdIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new CreateGroupInvitationCommand { GroupId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }
}


