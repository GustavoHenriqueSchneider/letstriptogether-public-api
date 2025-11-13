using Application.UseCases.GroupDestinationVote.Query.GetGroupDestinationVoteById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupDestinationVote.Query.GetGroupDestinationVoteById;

[TestFixture]
public class GetGroupDestinationVoteByIdValidatorTests
{
    private GetGroupDestinationVoteByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetGroupDestinationVoteByIdValidator();
    }

    [Test]
    public void Validate_WhenQueryIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetGroupDestinationVoteByIdQuery
        {
            GroupId = Guid.NewGuid(),
            DestinationVoteId = Guid.NewGuid()
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
        var query = new GetGroupDestinationVoteByIdQuery
        {
            GroupId = Guid.Empty,
            DestinationVoteId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }

    [Test]
    public void Validate_WhenDestinationVoteIdIsEmpty_ShouldFail()
    {
        // Arrange
        var query = new GetGroupDestinationVoteByIdQuery
        {
            GroupId = Guid.NewGuid(),
            DestinationVoteId = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DestinationVoteId)
            .WithErrorMessage("'Destination Vote Id' must not be empty.");
    }
}


