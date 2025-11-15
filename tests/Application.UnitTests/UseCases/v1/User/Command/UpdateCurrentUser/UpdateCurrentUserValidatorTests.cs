using Application.UseCases.v1.User.Command.UpdateCurrentUser;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.User.Command.UpdateCurrentUser;

[TestFixture]
public class UpdateCurrentUserValidatorTests
{
    private UpdateCurrentUserValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateCurrentUserValidator();
    }

    [Test]
    public void Validate_WhenNameIsValid_ShouldPass()
    {
        // Arrange
        var command = new UpdateCurrentUserCommand { Name = "John Doe" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenNameIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new UpdateCurrentUserCommand { Name = string.Empty };

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
        var command = new UpdateCurrentUserCommand 
        { 
            Name = new string('A', UpdateCurrentUserValidator.NameMaxLength + 1) 
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Validate_WhenNameIsAtMaxLength_ShouldPass()
    {
        // Arrange
        var command = new UpdateCurrentUserCommand 
        { 
            Name = new string('A', UpdateCurrentUserValidator.NameMaxLength) 
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}


