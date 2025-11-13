using Application.Common.Behaviours;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using NUnit.Framework;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.UnitTests.Common.Behaviours;

[TestFixture]
public class ValidationBehaviourTests
{
    private Mock<IValidator<TestRequest>> _validatorMock = null!;
    private ValidationBehaviour<TestRequest, TestResponse> _behaviour = null!;

    [SetUp]
    public void SetUp()
    {
        _validatorMock = new Mock<IValidator<TestRequest>>();
        var validators = new List<IValidator<TestRequest>> { _validatorMock.Object };
        _behaviour = new ValidationBehaviour<TestRequest, TestResponse>(validators);
    }

    [Test]
    public async Task Handle_WhenNoValidators_ShouldCallNext()
    {
        // Arrange
        var emptyValidators = new List<IValidator<TestRequest>>();
        var behaviour = new ValidationBehaviour<TestRequest, TestResponse>(emptyValidators);
        var request = new TestRequest { Name = "Test" };
        var expectedResponse = new TestResponse { Id = 1 };
        var nextCalled = false;

        RequestHandlerDelegate<TestResponse> next = () =>
        {
            nextCalled = true;
            return Task.FromResult(expectedResponse);
        };

        // Act
        var result = await behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        nextCalled.Should().BeTrue();
    }

    [Test]
    public async Task Handle_WhenValidationPasses_ShouldCallNext()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        var expectedResponse = new TestResponse { Id = 1 };
        var validationResult = new FluentValidation.Results.ValidationResult();

        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var nextCalled = false;
        RequestHandlerDelegate<TestResponse> next = () =>
        {
            nextCalled = true;
            return Task.FromResult(expectedResponse);
        };

        // Act
        var result = await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        nextCalled.Should().BeTrue();
        _validatorMock.Verify(
            x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var request = new TestRequest { Name = "" };
        var validationErrors = new List<FluentValidation.Results.ValidationFailure>
        {
            new("Name", "Name is required"),
            new("Email", "Email is invalid")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationErrors);

        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(new TestResponse { Id = 1 });

        // Act
        Func<Task> act = async () => await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().HaveCount(2);
        exception.Which.Errors.Should().ContainKey("Name");
        exception.Which.Errors.Should().ContainKey("Email");
        exception.Which.Errors["Name"].Should().Contain("Name is required");
        exception.Which.Errors["Email"].Should().Contain("Email is invalid");
    }

    [Test]
    public async Task Handle_WhenMultipleValidators_ShouldCallAllValidators()
    {
        // Arrange
        var validator1 = new Mock<IValidator<TestRequest>>();
        var validator2 = new Mock<IValidator<TestRequest>>();
        var validators = new List<IValidator<TestRequest>> { validator1.Object, validator2.Object };
        var behaviour = new ValidationBehaviour<TestRequest, TestResponse>(validators);

        var request = new TestRequest { Name = "Test" };
        var validationResult = new FluentValidation.Results.ValidationResult();

        validator1
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        validator2
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(new TestResponse { Id = 1 });

        // Act
        await behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        validator1.Verify(
            x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()),
            Times.Once);
        validator2.Verify(
            x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenValidationErrorsGroupedByProperty_ShouldGroupCorrectly()
    {
        // Arrange
        var request = new TestRequest { Name = "" };
        var validationErrors = new List<FluentValidation.Results.ValidationFailure>
        {
            new("Name", "Name is required"),
            new("Name", "Name must be at least 3 characters"),
            new("Email", "Email is invalid")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationErrors);

        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(new TestResponse { Id = 1 });

        // Act
        Func<Task> act = async () => await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().HaveCount(2);
        exception.Which.Errors["Name"].Should().HaveCount(2);
        exception.Which.Errors["Name"].Should().Contain("Name is required");
        exception.Which.Errors["Name"].Should().Contain("Name must be at least 3 characters");
        exception.Which.Errors["Email"].Should().HaveCount(1);
    }

    [Test]
    public async Task Handle_WhenPropertyNameIsNull_ShouldUseEmptyStringAsKey()
    {
        // Arrange
        var request = new TestRequest { Name = "" };
        var validationErrors = new List<FluentValidation.Results.ValidationFailure>
        {
            new("", "General error")
        };
        var validationResult = new FluentValidation.Results.ValidationResult(validationErrors);

        _validatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(new TestResponse { Id = 1 });

        // Act
        Func<Task> act = async () => await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().ContainKey(string.Empty);
    }

    public class TestRequest : MediatR.IBaseRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class TestResponse
    {
        public int Id { get; set; }
    }
}
