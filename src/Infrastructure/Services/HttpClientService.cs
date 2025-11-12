using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Services;
using Domain.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    public HttpClientService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private void SetAuthentication(HttpRequestMessage httpRequestMessage)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Request.Headers.ContainsKey("Authorization") != true)
        {
            return;
        }

        var authorizationHeader = httpContext.Request.Headers.Authorization.ToString();
        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            httpRequestMessage.Headers.Add("Authorization", authorizationHeader);
        }
    }
    
    private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, string url, 
        IBaseRequest? request = null, CancellationToken cancellationToken = default)
    {
        var httpRequestMessage = method.ToString() switch
        {
            "GET" => new HttpRequestMessage(HttpMethod.Get, url),
            "POST" => new HttpRequestMessage(HttpMethod.Post, url),
            "PUT" => new HttpRequestMessage(HttpMethod.Put, url),
            "DELETE" => new HttpRequestMessage(HttpMethod.Delete, url),
            _ => new HttpRequestMessage(HttpMethod.Patch, url)
        };

        if (request is not null)
        {
            var json = JsonSerializer.Serialize(request, request.GetType(), JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpRequestMessage.Content = content;
        }
        
        SetAuthentication(httpRequestMessage);
        
        var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);

        try
        {
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (HttpRequestException ex)
        {
            var content = await GetContentAsync<BaseException>(response, cancellationToken);

            throw (int?)ex.StatusCode switch
            {
                400 => new BadRequestException(content.Message),
                401 => new UnauthorizedAccessException(content.Message),
                403 => new ForbiddenException(content.Message),
                404 => new NotFoundException(content.Message),
                409 => new ConflictException(content.Message),
                415 => new UnsupportedMediaTypeException(content.Message),
                422 => new DomainBusinessRuleException(content.Message, content.Title),
                _ => new InternalServerErrorException(content.Message)
            };
        }
    }
    
    private async Task<T> GetContentAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
               ?? throw new ArgumentNullException("Response must be returned", nameof(response.Content));
    }
    
    public async Task<T> PostAsync<T>(string url, IBaseRequest request,
        CancellationToken cancellationToken) where T : class
    {
        var response = await SendRequestAsync(HttpMethod.Post, url, request, cancellationToken);
        return await GetContentAsync<T>(response, cancellationToken);
    }

    public async Task PostAsync(string url, IBaseRequest request, CancellationToken cancellationToken)
    {
        await SendRequestAsync(HttpMethod.Post, url, request, cancellationToken);
    }

    public async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken) where T : class
    {
        var response = await SendRequestAsync(HttpMethod.Get, url, cancellationToken: cancellationToken);
        return await GetContentAsync<T>(response, cancellationToken);
    }

    public async Task PutAsync(string url, IBaseRequest request, CancellationToken cancellationToken)
    {
        await SendRequestAsync(HttpMethod.Put, url, request, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string url, CancellationToken cancellationToken)
    {
        await SendRequestAsync(HttpMethod.Delete, url, cancellationToken: cancellationToken);
    }

    public async Task PatchAsync(string url, IBaseRequest request, CancellationToken cancellationToken)
    {
        await SendRequestAsync(HttpMethod.Patch, url, request, cancellationToken: cancellationToken);
    }
}
