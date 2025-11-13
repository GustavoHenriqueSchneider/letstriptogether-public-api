using Application.UseCases.GroupMatch.Query.GetGroupMatchById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupMatch.Query.GetGroupMatchById;

[TestFixture]
public class GetGroupMatchByIdValidatorTests
{
    private GetGroupMatchByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetGroupMatchByIdValidator();
    }

    [Test]
    public void Validate_WhenQueryIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetGroupMatchByIdQuery
        {
            GroupId = Guid.NewGuid(),
            MatchId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenGroupIdIsEmpty_ShouldFail()
    {
        // Arrange
        var query = new GetGroupMatchByIdQuery
        {
            GroupId = Guid.Empty,
            MatchId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }

    [Test]
    public void Validate_WhenMatchIdIsEmpty_ShouldFail()
    {
        // Arrange
        var query = new GetGroupMatchByIdQuery
        {
            GroupId = Guid.NewGuid(),
            MatchId = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MatchId)
            .WithErrorMessage("'Match Id' must not be empty.");
    }
}


