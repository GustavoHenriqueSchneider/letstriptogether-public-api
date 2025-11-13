using Application.Common.Validators;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.Common.Validators;

[TestFixture]
public class FoodPreferencesValidatorTests
{
    private FoodPreferencesValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new FoodPreferencesValidator();
    }

    [Test]
    public void Validate_WhenPreferencesAreValid_ShouldPass()
    {
        // Arrange
        var preferences = new[] { "Italian", "Japanese", "Mexican" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenPreferencesIsEmpty_ShouldFail()
    {
        // Arrange
        var preferences = Array.Empty<string>();

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("must not be empty"));
    }

    [Test]
    public void Validate_WhenPreferencesIsNull_ShouldFail()
    {
        // Arrange
        IEnumerable<string>? preferences = null;

        // Act & Assert
        Assert.Throws<System.ArgumentNullException>(() => _validator.TestValidate(preferences!));
    }

    [Test]
    public void Validate_WhenContainsEmptyString_ShouldFail()
    {
        // Arrange
        var preferences = new[] { "Italian", "", "Mexican" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Test]
    public void Validate_WhenContainsWhitespaceOnly_ShouldFail()
    {
        // Arrange
        var preferences = new[] { "Italian", "   ", "Mexican" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }

    [Test]
    public void Validate_WhenAllItemsAreValid_ShouldPass()
    {
        // Arrange
        var preferences = new[] { "Italian", "Japanese", "Mexican", "French" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

[TestFixture]
public class CulturePreferencesValidatorTests
{
    private CulturePreferencesValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new CulturePreferencesValidator();
    }

    [Test]
    public void Validate_WhenPreferencesAreValid_ShouldPass()
    {
        // Arrange
        var preferences = new[] { "Museums", "Theaters", "Art Galleries" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenPreferencesIsEmpty_ShouldFail()
    {
        // Arrange
        var preferences = Array.Empty<string>();

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("must not be empty"));
    }

    [Test]
    public void Validate_WhenContainsEmptyString_ShouldFail()
    {
        // Arrange
        var preferences = new[] { "Museums", "", "Theaters" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }
}

[TestFixture]
public class EntertainmentPreferencesValidatorTests
{
    private EntertainmentPreferencesValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new EntertainmentPreferencesValidator();
    }

    [Test]
    public void Validate_WhenPreferencesAreValid_ShouldPass()
    {
        // Arrange
        var preferences = new[] { "Concerts", "Sports", "Nightlife" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenPreferencesIsEmpty_ShouldFail()
    {
        // Arrange
        var preferences = Array.Empty<string>();

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("must not be empty"));
    }

    [Test]
    public void Validate_WhenContainsEmptyString_ShouldFail()
    {
        // Arrange
        var preferences = new[] { "Concerts", "", "Sports" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }
}

[TestFixture]
public class PlaceTypePreferencesValidatorTests
{
    private PlaceTypePreferencesValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new PlaceTypePreferencesValidator();
    }

    [Test]
    public void Validate_WhenPreferencesAreValid_ShouldPass()
    {
        // Arrange
        var preferences = new[] { "Beach", "Mountain", "City" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenPreferencesIsEmpty_ShouldFail()
    {
        // Arrange
        var preferences = Array.Empty<string>();

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains("must not be empty"));
    }

    [Test]
    public void Validate_WhenContainsEmptyString_ShouldFail()
    {
        // Arrange
        var preferences = new[] { "Beach", "", "Mountain" };

        // Act
        var result = _validator.TestValidate(preferences);

        // Assert
        result.Errors.Should().NotBeEmpty();
    }
}


