using Application.UseCases.v1.GroupMatch.Command.RemoveGroupMatchById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupMatch.Command.RemoveGroupMatchById;

[TestFixture]
public class RemoveGroupMatchByIdValidatorTests
{
    private RemoveGroupMatchByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new RemoveGroupMatchByIdValidator();
    }

    [Test]
    public void Validate_WhenCommandIsValid_ShouldPass()
    {
        // Arrange
        var command = new RemoveGroupMatchByIdCommand
        {
            GroupId = Guid.NewGuid(),
            MatchId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenGroupIdIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new RemoveGroupMatchByIdCommand
        {
            GroupId = Guid.Empty,
            MatchId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }

    [Test]
    public void Validate_WhenMatchIdIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new RemoveGroupMatchByIdCommand
        {
            GroupId = Guid.NewGuid(),
            MatchId = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MatchId)
            .WithErrorMessage("'Match Id' must not be empty.");
    }
}


