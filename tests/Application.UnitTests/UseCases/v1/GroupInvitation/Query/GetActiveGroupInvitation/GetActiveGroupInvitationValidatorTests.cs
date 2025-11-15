using Application.UseCases.v1.GroupInvitation.Query.GetActiveGroupInvitation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupInvitation.Query.GetActiveGroupInvitation;

[TestFixture]
public class GetActiveGroupInvitationValidatorTests
{
    private GetActiveGroupInvitationValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetActiveGroupInvitationValidator();
    }

    [Test]
    public void Validate_WhenQueryIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetActiveGroupInvitationQuery { GroupId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenGroupIdIsEmpty_ShouldFail()
    {
        // Arrange
        var query = new GetActiveGroupInvitationQuery { GroupId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }
}


