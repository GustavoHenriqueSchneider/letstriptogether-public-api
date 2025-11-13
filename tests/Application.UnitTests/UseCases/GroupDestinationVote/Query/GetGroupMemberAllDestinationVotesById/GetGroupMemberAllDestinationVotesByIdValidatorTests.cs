using Application.UseCases.GroupDestinationVote.Query.GetGroupMemberAllDestinationVotesById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupDestinationVote.Query.GetGroupMemberAllDestinationVotesById;

[TestFixture]
public class GetGroupMemberAllDestinationVotesByIdValidatorTests
{
    private GetGroupMemberAllDestinationVotesByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetGroupMemberAllDestinationVotesByIdValidator();
    }

    [Test]
    public void Validate_WhenQueryIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetGroupMemberAllDestinationVotesByIdQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 10
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
        var query = new GetGroupMemberAllDestinationVotesByIdQuery
        {
            GroupId = Guid.Empty,
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }

    [Test]
    public void Validate_WhenPageNumberIsZero_ShouldFail()
    {
        // Arrange
        var query = new GetGroupMemberAllDestinationVotesByIdQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 0,
            PageSize = 10
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
            .WithErrorMessage("'Page Number' must be greater than '0'.");
    }

    [Test]
    public void Validate_WhenPageNumberIsNegative_ShouldFail()
    {
        // Arrange
        var query = new GetGroupMemberAllDestinationVotesByIdQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = -1,
            PageSize = 10
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber)
            .WithErrorMessage("'Page Number' must be greater than '0'.");
    }

    [Test]
    public void Validate_WhenPageSizeIsZero_ShouldFail()
    {
        // Arrange
        var query = new GetGroupMemberAllDestinationVotesByIdQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 0
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorMessage("'Page Size' must be greater than '0'.");
    }

    [Test]
    public void Validate_WhenPageSizeIsNegative_ShouldFail()
    {
        // Arrange
        var query = new GetGroupMemberAllDestinationVotesByIdQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = -1
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorMessage("'Page Size' must be greater than '0'.");
    }
}
