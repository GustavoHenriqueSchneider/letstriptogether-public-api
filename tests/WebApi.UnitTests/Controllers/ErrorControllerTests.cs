using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
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
    private Mock<IExceptionHandlerPathFeature> _exceptionHandlerPathFeatureMock = null!;
    private Mock<HttpContext> _httpContextMock = null!;
    private Mock<IFeatureCollection> _featureCollectionMock = null!;

    [SetUp]
    public void SetUp()
    {
        _controller = new ErrorController();
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
    public void Error_WhenBaseException_ShouldReturnProblemDetailsWithStatusCode()
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

        // Act
        var result = _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(400);
        var problemDetails = result.Value as ProblemDetails;
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(400);
        problemDetails.Title.Should().Be("Bad Request");
        problemDetails.Detail.Should().Be("Invalid request");
        problemDetails.Instance.Should().Be("/api/test");
    }

    [Test]
    public void Error_WhenValidationException_ShouldReturnProblemDetailsWithErrors()
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

        // Act
        var result = _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        var problemDetails = result!.Value as ProblemDetails;
        problemDetails.Should().NotBeNull();
        problemDetails!.Extensions.Should().ContainKey("errors");
        problemDetails.Extensions["errors"].Should().BeEquivalentTo(errors);
    }

    [Test]
    public void Error_WhenDomainBusinessRuleException_ShouldReturnProblemDetails()
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

        // Act
        var result = _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(422);
        var problemDetails = result.Value as ProblemDetails;
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(422);
        problemDetails.Title.Should().Be("Business Rule Violation");
        problemDetails.Detail.Should().Be("Business rule violated");
    }

    [Test]
    public void Error_WhenGenericException_ShouldReturnInternalServerError()
    {
        // Arrange
        var exception = new Exception("Something went wrong");

        _exceptionHandlerPathFeatureMock.Setup(x => x.Error).Returns(exception);
        _exceptionHandlerPathFeatureMock.Setup(x => x.Path).Returns("/api/test");
        _featureCollectionMock
            .Setup(x => x.Get<IExceptionHandlerPathFeature>())
            .Returns(_exceptionHandlerPathFeatureMock.Object);

        // Act
        var result = _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(500);
        var problemDetails = result.Value as ProblemDetails;
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(500);
        problemDetails.Title.Should().Be("An error occurred while processing your request");
    }

    [Test]
    public void Error_WhenBaseExceptionHasNoTitle_ShouldUseDefaultTitle()
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

        // Act
        var result = _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        var problemDetails = result!.Value as ProblemDetails;
        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be("");
    }

    [Test]
    public void Error_WhenExceptionHandlerPathFeatureIsNull_ShouldReturnInternalServerError()
    {
        // Arrange
        _featureCollectionMock
            .Setup(x => x.Get<IExceptionHandlerPathFeature>())
            .Returns((IExceptionHandlerPathFeature?)null);

        // Act
        var result = _controller.Error() as ObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(500);
        var problemDetails = result.Value as ProblemDetails;
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(500);
        problemDetails.Instance.Should().BeEmpty();
    }
}
