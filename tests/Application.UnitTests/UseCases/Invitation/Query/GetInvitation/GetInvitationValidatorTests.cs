using Application.UseCases.Invitation.Query.GetInvitation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Invitation.Query.GetInvitation;

[TestFixture]
public class GetInvitationValidatorTests
{
    private GetInvitationValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetInvitationValidator();
    }

    [Test]
    public void Validate_WhenTokenIsEmpty_ShouldFail()
    {
        // Arrange
        var query = new GetInvitationQuery { Token = string.Empty };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Token);
    }

    [Test]
    public void Validate_WhenTokenIsProvided_ShouldPass()
    {
        // Arrange
        var query = new GetInvitationQuery { Token = "token-value" };

        // Act & Assert
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}



