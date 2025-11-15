using Application.UseCases.v1.Group.Query.GetGroupById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Group.Query.GetGroupById;

[TestFixture]
public class GetGroupByIdValidatorTests
{
    private GetGroupByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetGroupByIdValidator();
    }

    [Test]
    public void Validate_WhenGroupIdIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetGroupByIdQuery { GroupId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenGroupIdIsEmpty_ShouldFail()
    {
        // Arrange
        var query = new GetGroupByIdQuery { GroupId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }
}


