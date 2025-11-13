using Application.UseCases.GroupDestinationVote.Command.UpdateDestinationVoteById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupDestinationVote.Command.UpdateDestinationVoteById;

[TestFixture]
public class UpdateDestinationVoteByIdValidatorTests
{
    private UpdateDestinationVoteByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateDestinationVoteByIdValidator();
    }

    [Test]
    public void Validate_WhenCommandIsValid_ShouldPass()
    {
        // Arrange
        var command = new UpdateDestinationVoteByIdCommand
        {
            GroupId = Guid.NewGuid(),
            DestinationVoteId = Guid.NewGuid(),
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
        var command = new UpdateDestinationVoteByIdCommand
        {
            GroupId = Guid.Empty,
            DestinationVoteId = Guid.NewGuid(),
            IsApproved = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }

    [Test]
    public void Validate_WhenDestinationVoteIdIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new UpdateDestinationVoteByIdCommand
        {
            GroupId = Guid.NewGuid(),
            DestinationVoteId = Guid.Empty,
            IsApproved = true
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DestinationVoteId)
            .WithErrorMessage("'Destination Vote Id' must not be empty.");
    }
}


