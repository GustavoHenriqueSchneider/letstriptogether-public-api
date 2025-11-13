using Application.UseCases.Group.Query.GetAllGroups;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Group.Query.GetAllGroups;

[TestFixture]
public class GetAllGroupsValidatorTests
{
    private GetAllGroupsValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetAllGroupsValidator();
    }

    [Test]
    public void Validate_WhenQueryIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetAllGroupsQuery { PageNumber = 1, PageSize = 10 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenPageNumberIsZero_ShouldFail()
    {
        // Arrange
        var query = new GetAllGroupsQuery { PageNumber = 0, PageSize = 10 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Test]
    public void Validate_WhenPageNumberIsNegative_ShouldFail()
    {
        // Arrange
        var query = new GetAllGroupsQuery { PageNumber = -1, PageSize = 10 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Test]
    public void Validate_WhenPageSizeIsZero_ShouldFail()
    {
        // Arrange
        var query = new GetAllGroupsQuery { PageNumber = 1, PageSize = 0 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Test]
    public void Validate_WhenPageSizeIsNegative_ShouldFail()
    {
        // Arrange
        var query = new GetAllGroupsQuery { PageNumber = 1, PageSize = -1 };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }
}


