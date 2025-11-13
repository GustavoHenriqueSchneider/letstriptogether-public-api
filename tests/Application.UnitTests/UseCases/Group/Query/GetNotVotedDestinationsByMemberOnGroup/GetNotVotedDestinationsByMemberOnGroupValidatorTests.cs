using Application.UseCases.Group.Query.GetNotVotedDestinationsByMemberOnGroup;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Group.Query.GetNotVotedDestinationsByMemberOnGroup;

[TestFixture]
public class GetNotVotedDestinationsByMemberOnGroupValidatorTests
{
    private GetNotVotedDestinationsByMemberOnGroupValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetNotVotedDestinationsByMemberOnGroupValidator();
    }

    [Test]
    public void Validate_WhenQueryIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetNotVotedDestinationsByMemberOnGroupQuery
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
        var query = new GetNotVotedDestinationsByMemberOnGroupQuery
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
        var query = new GetNotVotedDestinationsByMemberOnGroupQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 0,
            PageSize = 10
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Test]
    public void Validate_WhenPageSizeIsZero_ShouldFail()
    {
        // Arrange
        var query = new GetNotVotedDestinationsByMemberOnGroupQuery
        {
            GroupId = Guid.NewGuid(),
            PageNumber = 1,
            PageSize = 0
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }
}


