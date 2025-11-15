using Application.UseCases.v1.Group.Command.CreateGroup;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Group.Command.CreateGroup;

[TestFixture]
public class CreateGroupValidatorTests
{
    private CreateGroupValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateGroupValidator();
    }

    [Test]
    public void Validate_WhenCommandIsValid_ShouldPass()
    {
        // Arrange
        var command = new CreateGroupCommand
        {
            Name = "My Group",
            TripExpectedDate = DateTime.UtcNow.AddDays(30)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenNameIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new CreateGroupCommand
        {
            Name = string.Empty,
            TripExpectedDate = DateTime.UtcNow.AddDays(30)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("'Name' must not be empty.");
    }

    [Test]
    public void Validate_WhenNameExceedsMaxLength_ShouldFail()
    {
        // Arrange
        var command = new CreateGroupCommand
        {
            Name = new string('A', CreateGroupValidator.NameMaxLength + 1),
            TripExpectedDate = DateTime.UtcNow.AddDays(30)
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
        var command = new CreateGroupCommand
        {
            Name = "My Group",
            TripExpectedDate = DateTime.UtcNow.AddDays(-1)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TripExpectedDate);
    }

    [Test]
    public void Validate_WhenTripExpectedDateIsToday_ShouldFail()
    {
        // Arrange
        var command = new CreateGroupCommand
        {
            Name = "My Group",
            TripExpectedDate = DateTime.UtcNow.AddMilliseconds(-1)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TripExpectedDate);
    }
}


