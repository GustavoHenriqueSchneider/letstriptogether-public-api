using Application.UseCases.Destination.Query.GetDestinationById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.Destination.Query.GetDestinationById;

[TestFixture]
public class GetDestinationByIdValidatorTests
{
    private GetDestinationByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetDestinationByIdValidator();
    }

    [Test]
    public void Validate_WhenDestinationIdIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetDestinationByIdQuery { DestinationId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenDestinationIdIsEmpty_ShouldFail()
    {
        // Arrange
        var query = new GetDestinationByIdQuery { DestinationId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DestinationId)
            .WithErrorMessage("'Destination Id' must not be empty.");
    }
}


