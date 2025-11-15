using Application.UseCases.v1.Notification.Command.ProcessNotification;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Application.UnitTests.UseCases.v1.Notification.Command.ProcessNotification;

[TestFixture]
public class ProcessNotificationValidatorTests
{
    private ProcessNotificationValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new ProcessNotificationValidator();
    }

    [Test]
    public void Validate_WhenValidRequest_ShouldPass()
    {
        // Arrange
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = "TestEvent",
            Data = new { Test = "Data" },
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validate_WhenUserIdIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.Empty,
            EventName = "TestEvent",
            Data = new { Test = "Data" },
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public void Validate_WhenEventNameIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = string.Empty,
            Data = new { Test = "Data" },
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EventName);
    }

    [Test]
    public void Validate_WhenEventNameIsNull_ShouldFail()
    {
        // Arrange
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = null!,
            Data = new { Test = "Data" },
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EventName);
    }

    [Test]
    public void Validate_WhenDataIsNull_ShouldFail()
    {
        // Arrange
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = "TestEvent",
            Data = null!,
            CreatedAt = DateTime.UtcNow
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Data);
    }

    [Test]
    public void Validate_WhenCreatedAtIsEmpty_ShouldFail()
    {
        // Arrange
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.NewGuid(),
            EventName = "TestEvent",
            Data = new { Test = "Data" },
            CreatedAt = default(DateTime)
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CreatedAt);
    }

    [Test]
    public void Validate_WhenAllFieldsAreInvalid_ShouldFail()
    {
        // Arrange
        var command = new ProcessNotificationCommand
        {
            UserId = Guid.Empty,
            EventName = string.Empty,
            Data = null!,
            CreatedAt = default(DateTime)
        };

        // Act & Assert
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.EventName);
        result.ShouldHaveValidationErrorFor(x => x.Data);
        result.ShouldHaveValidationErrorFor(x => x.CreatedAt);
    }
}
