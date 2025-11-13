using Application.UseCases.User.Command.SetCurrentUserPreferences;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.User.Command.SetCurrentUserPreferences;

[TestFixture]
public class SetCurrentUserPreferencesValidatorTests
{
    private SetCurrentUserPreferencesValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new SetCurrentUserPreferencesValidator();
    }

    [Test]
    public void Validate_WhenAllPreferencesAreValid_ShouldPass()
    {
        // Arrange
        var command = new SetCurrentUserPreferencesCommand
        {
            LikesCommercial = true,
            Food = new List<string> { "Italian", "Japanese" },
            Culture = new List<string> { "Museums", "Theaters" },
            Entertainment = new List<string> { "Concerts", "Sports" },
            PlaceTypes = new List<string> { "Beach", "Mountain" }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenFoodIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new SetCurrentUserPreferencesCommand
        {
            LikesCommercial = true,
            Food = new List<string>(),
            Culture = new List<string> { "Museums" },
            Entertainment = new List<string> { "Concerts" },
            PlaceTypes = new List<string> { "Beach" }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Food);
    }

    [Test]
    public void Validate_WhenCultureIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new SetCurrentUserPreferencesCommand
        {
            LikesCommercial = true,
            Food = new List<string> { "Italian" },
            Culture = new List<string>(),
            Entertainment = new List<string> { "Concerts" },
            PlaceTypes = new List<string> { "Beach" }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Culture);
    }

    [Test]
    public void Validate_WhenEntertainmentIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new SetCurrentUserPreferencesCommand
        {
            LikesCommercial = true,
            Food = new List<string> { "Italian" },
            Culture = new List<string> { "Museums" },
            Entertainment = new List<string>(),
            PlaceTypes = new List<string> { "Beach" }
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Entertainment);
    }

    [Test]
    public void Validate_WhenPlaceTypesIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new SetCurrentUserPreferencesCommand
        {
            LikesCommercial = true,
            Food = new List<string> { "Italian" },
            Culture = new List<string> { "Museums" },
            Entertainment = new List<string> { "Concerts" },
            PlaceTypes = new List<string>()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlaceTypes);
    }
}


