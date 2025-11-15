using Application.UseCases.v1.GroupDestinationVote.Command.VoteAtDestinationForGroupId;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.GroupDestinationVote.Command.VoteAtDestinationForGroupId;

[TestFixture]
public class VoteAtDestinationForGroupIdValidatorTests
{
    private VoteAtDestinationForGroupIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new VoteAtDestinationForGroupIdValidator();
    }

    [Test]
    public void Validate_WhenCommandIsValid_ShouldPass()
    {
        // Arrange
        var command = new VoteAtDestinationForGroupIdCommand
        {
            GroupId = Guid.NewGuid(),
            DestinationId = Guid.NewGuid(),
            IsApproved = true
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
        var command = new VoteAtDestinationForGroupIdCommand
        {
            GroupId = Guid.Empty,
            DestinationId = Guid.NewGuid(),
            IsApproved = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }

    [Test]
    public void Validate_WhenDestinationIdIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new VoteAtDestinationForGroupIdCommand
        {
            GroupId = Guid.NewGuid(),
            DestinationId = Guid.Empty,
            IsApproved = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DestinationId)
            .WithErrorMessage("'Destination Id' must not be empty.");
    }
}


