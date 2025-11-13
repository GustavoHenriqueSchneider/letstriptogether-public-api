using Application.UseCases.GroupMatch.Query.GetAllGroupMatchesById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupMatch.Query.GetAllGroupMatchesById;

[TestFixture]
public class GetAllGroupMatchesByIdValidatorTests
{
    private GetAllGroupMatchesByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetAllGroupMatchesByIdValidator();
    }

    [Test]
    public void Validate_WhenQueryIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetAllGroupMatchesByIdQuery
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
        var query = new GetAllGroupMatchesByIdQuery
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
        var query = new GetAllGroupMatchesByIdQuery
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
    public void Validate_WhenPageSizeIsZero_ShouldFail()
    {
        // Arrange
        var query = new GetAllGroupMatchesByIdQuery
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
}


