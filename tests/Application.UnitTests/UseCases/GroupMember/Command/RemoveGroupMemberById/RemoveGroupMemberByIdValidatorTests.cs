using Application.UseCases.GroupMember.Command.RemoveGroupMemberById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupMember.Command.RemoveGroupMemberById;

[TestFixture]
public class RemoveGroupMemberByIdValidatorTests
{
    private RemoveGroupMemberByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new RemoveGroupMemberByIdValidator();
    }

    [Test]
    public void Validate_WhenCommandIsValid_ShouldPass()
    {
        // Arrange
        var command = new RemoveGroupMemberByIdCommand
        {
            GroupId = Guid.NewGuid(),
            MemberId = Guid.NewGuid()
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
        var command = new RemoveGroupMemberByIdCommand
        {
            GroupId = Guid.Empty,
            MemberId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }

    [Test]
    public void Validate_WhenMemberIdIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new RemoveGroupMemberByIdCommand
        {
            GroupId = Guid.NewGuid(),
            MemberId = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MemberId)
            .WithErrorMessage("'Member Id' must not be empty.");
    }
}


