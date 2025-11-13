using Application.UseCases.Auth.Command.RefreshToken;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Auth.Command.RefreshToken;

[TestFixture]
public class RefreshTokenValidatorTests
{
    private RefreshTokenValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new RefreshTokenValidator();
    }

    [Test]
    public void Validate_WhenRefreshTokenIsValid_ShouldPass()
    {
        // Arrange
        var command = new RefreshTokenCommand { RefreshToken = "valid-refresh-token" };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenRefreshTokenIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new RefreshTokenCommand { RefreshToken = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
            .WithErrorMessage("'Refresh Token' must not be empty.");
    }

    [Test]
    public void Validate_WhenRefreshTokenIsNull_ShouldFail()
    {
        // Arrange
        var command = new RefreshTokenCommand { RefreshToken = null! };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
            .WithErrorMessage("'Refresh Token' must not be empty.");
    }
}


