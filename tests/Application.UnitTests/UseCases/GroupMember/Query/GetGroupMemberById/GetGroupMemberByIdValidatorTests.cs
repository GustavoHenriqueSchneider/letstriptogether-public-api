using Application.UseCases.GroupMember.Query.GetGroupMemberById;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.GroupMember.Query.GetGroupMemberById;

[TestFixture]
public class GetGroupMemberByIdValidatorTests
{
    private GetGroupMemberByIdValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new GetGroupMemberByIdValidator();
    }

    [Test]
    public void Validate_WhenQueryIsValid_ShouldPass()
    {
        // Arrange
        var query = new GetGroupMemberByIdQuery
        {
            GroupId = Guid.NewGuid(),
            MemberId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenGroupIdIsEmpty_ShouldFail()
    {
        // Arrange
        var query = new GetGroupMemberByIdQuery
        {
            GroupId = Guid.Empty,
            MemberId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GroupId)
            .WithErrorMessage("'Group Id' must not be empty.");
    }

    [Test]
    public void Validate_WhenMemberIdIsEmpty_ShouldFail()
    {
        // Arrange
        var query = new GetGroupMemberByIdQuery
        {
            GroupId = Guid.NewGuid(),
            MemberId = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.MemberId)
            .WithErrorMessage("'Member Id' must not be empty.");
    }
}


