using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Services;
using Domain.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class HttpClientService(
    HttpClient httpClient, 
    IHttpContextAccessor httpContextAccessor) : IHttpClientService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    private void SetAuthentication(HttpRequestMessage httpRequestMessage)
    {
        var httpContext = httpContextAccessor.HttpContext;
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
    
    private static void SetRequestBody(HttpRequestMessage request, IBaseRequest? body)
    {
        if (body is null)
        {
            return;
        }
        
        var json = JsonSerializer.Serialize(body, body.GetType(), JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        request.Content = content;
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
        
        SetAuthentication(httpRequestMessage);
        SetRequestBody(httpRequestMessage, request);
        
        var response = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

        try
        {
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (HttpRequestException ex)
        {
            var content = await GetContentAsync<InternalApiException>(response, cancellationToken);

            throw (int?)ex.StatusCode switch
            {
                400 => new BadRequestException(content),
                401 => new UnauthorizedException(content),
                403 => new ForbiddenException(content),
                404 => new NotFoundException(content),
                409 => new ConflictException(content),
                415 => new UnsupportedMediaTypeException(content),
                422 => new DomainBusinessRuleException(content),
                _ => new InternalServerErrorException(content)
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
