using Application.UseCases.Group.Command.UpdateGroupById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Group.Command.UpdateGroupById;

[TestFixture]
public class UpdateGroupByIdValidatorTests
{
    private UpdateGroupByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateGroupByIdValidator();
    }

    [Test]
    public void Validate_WhenCommandIsValid_ShouldPass()
    {
        // Arrange
        var command = new UpdateGroupByIdCommand
        {
            GroupId = Guid.NewGuid(),
            Name = "Updated Name",
            TripExpectedDate = DateTime.UtcNow.AddDays(30)
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
        var command = new UpdateGroupByIdCommand
        {
            GroupId = Guid.Empty,
            Name = "Updated Name"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }

    [Test]
    public void Validate_WhenNameIsProvidedAndExceedsMaxLength_ShouldFail()
    {
        // Arrange
        var command = new UpdateGroupByIdCommand
        {
            GroupId = Guid.NewGuid(),
            Name = new string('A', UpdateGroupByIdValidator.NameMaxLength + 1)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validate_WhenTripExpectedDateIsInThePast_ShouldFail()
    {
        // Arrange
        var command = new UpdateGroupByIdCommand
        {
            GroupId = Guid.NewGuid(),
            TripExpectedDate = DateTime.UtcNow.AddDays(-1)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TripExpectedDate);
    }

    [Test]
    public void Validate_WhenNameIsNull_ShouldPass()
    {
        // Arrange
        var command = new UpdateGroupByIdCommand
        {
            GroupId = Guid.NewGuid(),
            Name = null
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenNameIsEmpty_ShouldPass()
    {
        // Arrange
        var command = new UpdateGroupByIdCommand
        {
            GroupId = Guid.NewGuid(),
            Name = string.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenTripExpectedDateIsNull_ShouldPass()
    {
        // Arrange
        var command = new UpdateGroupByIdCommand
        {
            GroupId = Guid.NewGuid(),
            TripExpectedDate = null
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}


