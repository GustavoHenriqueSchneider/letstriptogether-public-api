using Application.UseCases.v1.Group.Command.DeleteGroupById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Group.Command.DeleteGroupById;

[TestFixture]
public class DeleteGroupByIdValidatorTests
{
    private DeleteGroupByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteGroupByIdValidator();
    }

    [Test]
    public void Validate_WhenGroupIdIsValid_ShouldPass()
    {
        // Arrange
        var command = new DeleteGroupByIdCommand { GroupId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenGroupIdIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new DeleteGroupByIdCommand { GroupId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }
}


