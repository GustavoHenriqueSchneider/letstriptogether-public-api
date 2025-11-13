using Application.Common.Behaviours;
using Application.Common.Exceptions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Application.UnitTests.Common.Behaviours;

[TestFixture]
public class UnhandledExceptionBehaviourTests
{
    private Mock<ILogger<UnhandledExceptionBehaviour<TestRequest, TestResponse>>> _loggerMock = null!;
    private UnhandledExceptionBehaviour<TestRequest, TestResponse> _behaviour = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<UnhandledExceptionBehaviour<TestRequest, TestResponse>>>();
        _behaviour = new UnhandledExceptionBehaviour<TestRequest, TestResponse>(_loggerMock.Object);
    }

    [Test]
    public async Task Handle_WhenNoException_ShouldReturnResponse()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        var expectedResponse = new TestResponse { Id = 1 };

        RequestHandlerDelegate<TestResponse> next = () => Task.FromResult(expectedResponse);

        // Act
        var result = await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        _loggerMock.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Test]
    public async Task Handle_WhenBaseException_ShouldLogWarningAndRethrow()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        var baseException = new BadRequestException(
            new Domain.Common.Exceptions.InternalApiException
            {
                Title = "Bad Request",
                Status = 400,
                Detail = "Invalid request"
            });

        RequestHandlerDelegate<TestResponse> next = () => throw baseException;

        // Act
        Func<Task> act = async () => await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<BadRequestException>();
        exception.Which.Should().Be(baseException);

        var warningInvocations = _loggerMock.Invocations
            .Where(i => i.Method.Name == "Log" && 
                        i.Arguments.Count >= 4 && 
                        i.Arguments[0] is LogLevel && 
                        (LogLevel)i.Arguments[0] == LogLevel.Warning &&
                        i.Arguments[3] == null);
        warningInvocations.Should().HaveCount(1);
    }

    [Test]
    public async Task Handle_WhenUnhandledException_ShouldLogErrorAndRethrow()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        var unhandledException = new InvalidOperationException("Something went wrong");

        RequestHandlerDelegate<TestResponse> next = () => throw unhandledException;

        // Act
        Func<Task> act = async () => await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<InvalidOperationException>();
        exception.Which.Should().Be(unhandledException);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                unhandledException,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenBaseException_ShouldLogCorrectMessage()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        var baseException = new NotFoundException(
            new Domain.Common.Exceptions.InternalApiException
            {
                Title = "Not Found",
                Status = 404,
                Detail = "Resource not found"
            });

        RequestHandlerDelegate<TestResponse> next = () => throw baseException;

        // Act
        Func<Task> act = async () => await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        var warningInvocations = _loggerMock.Invocations
            .Where(i => i.Method.Name == "Log" && 
                        i.Arguments.Count >= 4 && 
                        i.Arguments[0] is LogLevel && 
                        (LogLevel)i.Arguments[0] == LogLevel.Warning &&
                        i.Arguments[3] == null);
        warningInvocations.Should().HaveCount(1);
    }

    [Test]
    public async Task Handle_WhenUnhandledException_ShouldLogRequestType()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        var unhandledException = new ArgumentException("Invalid argument");

        RequestHandlerDelegate<TestResponse> next = () => throw unhandledException;

        // Act
        Func<Task> act = async () => await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => 
                    v.ToString()!.Contains("Unhandled exception in handler") &&
                    v.ToString()!.Contains("TestRequest") &&
                    v.ToString()!.Contains("Invalid argument")),
                unhandledException,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task Handle_WhenMultipleBaseExceptions_ShouldLogEachAsWarning()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        var exception1 = new BadRequestException(
            new Domain.Common.Exceptions.InternalApiException
            {
                Title = "Bad Request",
                Status = 400,
                Detail = "Invalid"
            });

        RequestHandlerDelegate<TestResponse> next = () => throw exception1;

        // Act
        Func<Task> act = async () => await _behaviour.Handle(request, next, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    public class TestRequest : IRequest<TestResponse>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class TestResponse
    {
        public int Id { get; set; }
    }
}
