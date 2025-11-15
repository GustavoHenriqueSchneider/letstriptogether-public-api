using Application.Common.Exceptions;
using Application.UseCases.Error.Query.GetError;
using Domain.Common.Exceptions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;

namespace WebApi.UnitTests.Controllers;

[TestFixture]
public class ErrorControllerTests
{
    private ErrorController _controller = null!;
    private Mock<IMediator> _mediatorMock = null!;
    private Mock<IExceptionHandlerPathFeature> _exceptionHandlerPathFeatureMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IFeatureCollection> _featureCollectionMock = null!;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ErrorController(_mediatorMock.Object);
        _exceptionHandlerPathFeatureMock = new Mock<IExceptionHandlerPathFeature>();
        _httpContextMock = new Mock<HttpContext>();
        _featureCollectionMock = new Mock<IFeatureCollection>();

        _httpContextMock.Setup(x => x.Features).Returns(_featureCollectionMock.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContextMock.Object
        };
    }

    [Test]
    public async Task Error_WhenBaseException_ShouldReturnProblemDetailsWithStatusCode()
    {
        // Arrange
        var exception = new BadRequestException(
            new InternalApiException
            {
                Title = "Bad Request",
                Status = 400,
                Detail = "Invalid request"
            });

        _exceptionHandlerPathFeatureMock.Setup(x => x.Error).Returns(exception);
        _exceptionHandlerPathFeatureMock.Setup(x => x.Path).Returns("/api/test");
        _featureCollectionMock
            .Setup(x => x.Get<IExceptionHandlerPathFeature>())
            .Returns(_exceptionHandlerPathFeatureMock.Object);

        var expectedResponse = new GetErrorResponse
        {
            Status = 400,
            Title = "Bad Request",
            Detail = "Invalid request",
            Instance = "/api/test"
        };

        _mediatorMock
            .Setup(x => x.Send(It.Is<GetErrorQuery>(q => 
                q.Exception == exception && q.Path == "/api/test"), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(400);
        var response = result.Value as GetErrorResponse;
        response.Should().NotBeNull();
        response!.Status.Should().Be(400);
        response.Title.Should().Be("Bad Request");
        response.Detail.Should().Be("Invalid request");
        response.Instance.Should().Be("/api/test");
    }

    [Test]
    public async Task Error_WhenValidationException_ShouldReturnProblemDetailsWithErrors()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Email", new[] { "Email is required" } },
            { "Password", new[] { "Password is too short" } }
        };
        var exception = new ValidationException(errors);

        _exceptionHandlerPathFeatureMock.Setup(x => x.Error).Returns(exception);
        _exceptionHandlerPathFeatureMock.Setup(x => x.Path).Returns("/api/test");
        _featureCollectionMock
            .Setup(x => x.Get<IExceptionHandlerPathFeature>())
            .Returns(_exceptionHandlerPathFeatureMock.Object);

        var expectedResponse = new GetErrorResponse
        {
            Status = 400,
            Title = "An error occurred",
            Detail = exception.Message,
            Instance = "/api/test",
            Extensions = new Dictionary<string, object> { { "errors", errors } }
        };

        _mediatorMock
            .Setup(x => x.Send(It.Is<GetErrorQuery>(q => 
                q.Exception == exception && q.Path == "/api/test"), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        var response = result!.Value as GetErrorResponse;
        response.Should().NotBeNull();
        response!.Extensions.Should().ContainKey("errors");
        response.Extensions!["errors"].Should().BeEquivalentTo(errors);
    }

    [Test]
    public async Task Error_WhenDomainBusinessRuleException_ShouldReturnProblemDetails()
    {
        // Arrange
        var exception = new DomainBusinessRuleException(
            new InternalApiException
            {
                Title = "Business Rule Violation",
                Status = 422,
                Detail = "Business rule violated"
            });

        _exceptionHandlerPathFeatureMock.Setup(x => x.Error).Returns(exception);
        _exceptionHandlerPathFeatureMock.Setup(x => x.Path).Returns("/api/test");
        _featureCollectionMock
            .Setup(x => x.Get<IExceptionHandlerPathFeature>())
            .Returns(_exceptionHandlerPathFeatureMock.Object);

        var expectedResponse = new GetErrorResponse
        {
            Status = 422,
            Title = "Business Rule Violation",
            Detail = "Business rule violated",
            Instance = "/api/test"
        };

        _mediatorMock
            .Setup(x => x.Send(It.Is<GetErrorQuery>(q => 
                q.Exception == exception && q.Path == "/api/test"), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(422);
        var response = result.Value as GetErrorResponse;
        response.Should().NotBeNull();
        response!.Status.Should().Be(422);
        response.Title.Should().Be("Business Rule Violation");
        response.Detail.Should().Be("Business rule violated");
    }

    [Test]
    public async Task Error_WhenGenericException_ShouldReturnInternalServerError()
    {
        // Arrange
        var exception = new Exception("Something went wrong");

        _exceptionHandlerPathFeatureMock.Setup(x => x.Error).Returns(exception);
        _exceptionHandlerPathFeatureMock.Setup(x => x.Path).Returns("/api/test");
        _featureCollectionMock
            .Setup(x => x.Get<IExceptionHandlerPathFeature>())
            .Returns(_exceptionHandlerPathFeatureMock.Object);

        var expectedResponse = new GetErrorResponse
        {
            Status = 500,
            Title = "An error occurred while processing your request",
            Detail = "Something went wrong",
            Instance = "/api/test"
        };

        _mediatorMock
            .Setup(x => x.Send(It.Is<GetErrorQuery>(q => 
                q.Exception == exception && q.Path == "/api/test"), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(500);
        var response = result.Value as GetErrorResponse;
        response.Should().NotBeNull();
        response!.Status.Should().Be(500);
        response.Title.Should().Be("An error occurred while processing your request");
    }

    [Test]
    public async Task Error_WhenBaseExceptionHasNoTitle_ShouldUseDefaultTitle()
    {
        // Arrange
        var exception = new NotFoundException(
            new InternalApiException
            {
                Title = string.Empty,
                Status = 404,
                Detail = "Not found"
            });

        _exceptionHandlerPathFeatureMock.Setup(x => x.Error).Returns(exception);
        _exceptionHandlerPathFeatureMock.Setup(x => x.Path).Returns("/api/test");
        _featureCollectionMock
            .Setup(x => x.Get<IExceptionHandlerPathFeature>())
            .Returns(_exceptionHandlerPathFeatureMock.Object);

        var expectedResponse = new GetErrorResponse
        {
            Status = 404,
            Title = "",
            Detail = "Not found",
            Instance = "/api/test"
        };

        _mediatorMock
            .Setup(x => x.Send(It.Is<GetErrorQuery>(q => 
                q.Exception == exception && q.Path == "/api/test"), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        var response = result!.Value as GetErrorResponse;
        response.Should().NotBeNull();
        response!.Title.Should().Be("");
    }

    [Test]
    public async Task Error_WhenExceptionHandlerPathFeatureIsNull_ShouldReturnInternalServerError()
    {
        // Arrange
        _featureCollectionMock
            .Setup(x => x.Get<IExceptionHandlerPathFeature>())
            .Returns((IExceptionHandlerPathFeature?)null);

        var expectedResponse = new GetErrorResponse
        {
            Status = 500,
            Title = "An error occurred while processing your request",
            Detail = null,
            Instance = string.Empty
        };

        _mediatorMock
            .Setup(x => x.Send(It.Is<GetErrorQuery>(q => 
                q.Exception == null && q.Path == string.Empty), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(500);
        var response = result.Value as GetErrorResponse;
        response.Should().NotBeNull();
        response!.Status.Should().Be(500);
        response.Instance.Should().BeEmpty();
    }
}
