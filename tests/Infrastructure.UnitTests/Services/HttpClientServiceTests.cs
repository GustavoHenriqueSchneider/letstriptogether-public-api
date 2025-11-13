using System.Net;
using System.Text;
using System.Text.Json;
using Application.Common.Exceptions;
using Domain.Common.Exceptions;
using FluentAssertions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Infrastructure.UnitTests.Services;

[TestFixture]
public class HttpClientServiceTests
{
    private Mock<HttpMessageHandler> _httpMessageHandlerMock = null!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;
    private HttpClient _httpClient = null!;
    private HttpClientService _httpClientService = null!;

    [SetUp]
    public void SetUp()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _httpClientService = new HttpClientService(_httpClient, _httpContextAccessorMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient?.Dispose();
    }

    [Test]
    public async Task GetAsync_WhenResponseIsSuccess_ShouldReturnDeserializedObject()
    {
        // Arrange
        var expectedResponse = new { Id = 1, Name = "Test" };
        var jsonResponse = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.OK, jsonResponse);

        // Act
        var result = await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Test");
    }

    [Test]
    public async Task PostAsync_WhenResponseIsSuccess_ShouldReturnDeserializedObject()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        var expectedResponse = new { Id = 1, Name = "Test" };
        var jsonResponse = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.Created, jsonResponse);

        // Act
        var result = await _httpClientService.PostAsync<TestResponse>("https://api.test.com/test", request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Test");
    }

    [Test]
    public async Task PostAsync_WhenNoReturnType_ShouldCompleteSuccessfully()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        SetupHttpResponse(HttpStatusCode.NoContent, string.Empty);

        // Act
        Func<Task> act = async () => await _httpClientService.PostAsync("https://api.test.com/test", request, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task PutAsync_WhenResponseIsSuccess_ShouldCompleteSuccessfully()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        SetupHttpResponse(HttpStatusCode.NoContent, string.Empty);

        // Act
        Func<Task> act = async () => await _httpClientService.PutAsync("https://api.test.com/test", request, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task DeleteAsync_WhenResponseIsSuccess_ShouldCompleteSuccessfully()
    {
        // Arrange
        SetupHttpResponse(HttpStatusCode.NoContent, string.Empty);

        // Act
        Func<Task> act = async () => await _httpClientService.DeleteAsync("https://api.test.com/test", CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task PatchAsync_WhenResponseIsSuccess_ShouldCompleteSuccessfully()
    {
        // Arrange
        var request = new TestRequest { Name = "Test" };
        SetupHttpResponse(HttpStatusCode.NoContent, string.Empty);

        // Act
        Func<Task> act = async () => await _httpClientService.PatchAsync("https://api.test.com/test", request, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task GetAsync_WhenResponseIsBadRequest_ShouldThrowBadRequestException()
    {
        // Arrange
        var errorResponse = new InternalApiException
        {
            Title = "Bad Request",
            Status = 400,
            Detail = "Invalid request"
        };
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.BadRequest, jsonResponse);

        // Act
        Func<Task> act = async () => await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<BadRequestException>();
        exception.Which.Message.Should().Be("Invalid request");
        exception.Which.Title.Should().Be("Bad Request");
        exception.Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task GetAsync_WhenResponseIsUnauthorized_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var errorResponse = new InternalApiException
        {
            Title = "Unauthorized",
            Status = 401,
            Detail = "Authentication required"
        };
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.Unauthorized, jsonResponse);

        // Act
        Func<Task> act = async () => await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<UnauthorizedException>();
        exception.Which.Message.Should().Be("Authentication required");
        exception.Which.Title.Should().Be("Unauthorized");
        exception.Which.StatusCode.Should().Be(401);
    }

    [Test]
    public async Task GetAsync_WhenResponseIsForbidden_ShouldThrowForbiddenException()
    {
        // Arrange
        var errorResponse = new InternalApiException
        {
            Title = "Forbidden",
            Status = 403,
            Detail = "Access denied"
        };
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.Forbidden, jsonResponse);

        // Act
        Func<Task> act = async () => await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<ForbiddenException>();
        exception.Which.Message.Should().Be("Access denied");
        exception.Which.Title.Should().Be("Forbidden");
        exception.Which.StatusCode.Should().Be(403);
    }

    [Test]
    public async Task GetAsync_WhenResponseIsNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var errorResponse = new InternalApiException
        {
            Title = "Not Found",
            Status = 404,
            Detail = "Resource not found"
        };
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.NotFound, jsonResponse);

        // Act
        Func<Task> act = async () => await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<NotFoundException>();
        exception.Which.Message.Should().Be("Resource not found");
        exception.Which.Title.Should().Be("Not Found");
        exception.Which.StatusCode.Should().Be(404);
    }

    [Test]
    public async Task GetAsync_WhenResponseIsConflict_ShouldThrowConflictException()
    {
        // Arrange
        var errorResponse = new InternalApiException
        {
            Title = "Conflict",
            Status = 409,
            Detail = "Resource conflict"
        };
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.Conflict, jsonResponse);

        // Act
        Func<Task> act = async () => await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<ConflictException>();
        exception.Which.Message.Should().Be("Resource conflict");
        exception.Which.Title.Should().Be("Conflict");
        exception.Which.StatusCode.Should().Be(409);
    }

    [Test]
    public async Task GetAsync_WhenResponseIsUnsupportedMediaType_ShouldThrowUnsupportedMediaTypeException()
    {
        // Arrange
        var errorResponse = new InternalApiException
        {
            Title = "Unsupported Media Type",
            Status = 415,
            Detail = "Media type not supported"
        };
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.UnsupportedMediaType, jsonResponse);

        // Act
        Func<Task> act = async () => await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<UnsupportedMediaTypeException>();
        exception.Which.Message.Should().Be("Media type not supported");
        exception.Which.Title.Should().Be("Unsupported Media Type");
        exception.Which.StatusCode.Should().Be(415);
    }

    [Test]
    public async Task GetAsync_WhenResponseIsUnprocessableEntity_ShouldThrowDomainBusinessRuleException()
    {
        // Arrange
        var errorResponse = new InternalApiException
        {
            Title = "Business Rule Violation",
            Status = 422,
            Detail = "Business rule violated"
        };
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse((HttpStatusCode)422, jsonResponse);

        // Act
        Func<Task> act = async () => await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<DomainBusinessRuleException>();
        exception.Which.Message.Should().Be("Business rule violated");
        exception.Which.Title.Should().Be("Business Rule Violation");
        exception.Which.StatusCode.Should().Be(422);
    }

    [Test]
    public async Task GetAsync_WhenResponseIsInternalServerError_ShouldThrowInternalServerErrorException()
    {
        // Arrange
        var errorResponse = new InternalApiException
        {
            Title = "Internal Server Error",
            Status = 500,
            Detail = "An error occurred"
        };
        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.InternalServerError, jsonResponse);

        // Act
        Func<Task> act = async () => await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<InternalServerErrorException>();
        exception.Which.Message.Should().Be("An error occurred");
        exception.Which.Title.Should().Be("Internal Server Error");
        exception.Which.StatusCode.Should().Be(500);
    }

    [Test]
    public async Task PostAsync_WhenRequestHasBody_ShouldSerializeAndSendRequest()
    {
        // Arrange
        var request = new TestRequest { Name = "Test Request" };
        var expectedResponse = new { Id = 1, Name = "Test Request" };
        var jsonResponse = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.Created, jsonResponse, verifyRequest: (req) =>
        {
            var content = req.Content?.ReadAsStringAsync().Result;
            content.Should().NotBeNull();
            content.Should().Contain("{\"name\":\"Test Request\"}");
        });

        // Act
        var result = await _httpClientService.PostAsync<TestResponse>("https://api.test.com/test", request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task GetAsync_WhenHttpContextHasAuthorization_ShouldAddAuthorizationHeader()
    {
        // Arrange
        var httpContextMock = new Mock<HttpContext>();
        var requestMock = new Mock<HttpRequest>();
        var headersMock = new Mock<IHeaderDictionary>();
        
        headersMock.Setup(h => h.ContainsKey("Authorization")).Returns(true);
        headersMock.Setup(h => h.Authorization).Returns("Bearer token123");
        requestMock.Setup(r => r.Headers).Returns(headersMock.Object);
        httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        var expectedResponse = new { Id = 1, Name = "Test" };
        var jsonResponse = JsonSerializer.Serialize(expectedResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        SetupHttpResponse(HttpStatusCode.OK, jsonResponse, verifyRequest: (req) =>
        {
            req.Headers.Authorization.Should().NotBeNull();
            req.Headers.Authorization!.ToString().Should().Be("Bearer token123");
        });

        // Act
        await _httpClientService.GetAsync<TestResponse>("https://api.test.com/test", CancellationToken.None);

        // Assert
        _httpMessageHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    private void SetupHttpResponse(
        HttpStatusCode statusCode,
        string content,
        Action<HttpRequestMessage>? verifyRequest = null)
    {
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
            {
                verifyRequest?.Invoke(request);

                var response = new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                };

                return response;
            });
    }

    private class TestRequest : MediatR.IBaseRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    private class TestResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
