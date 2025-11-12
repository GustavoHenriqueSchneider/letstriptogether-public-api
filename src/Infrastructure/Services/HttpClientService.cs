using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Application.Common.Interfaces.Services;
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

    private async Task<HttpResponseMessage> InternalPostAsync(string url, IBaseRequest request,
        CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(request, request.GetType(), JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };

        SetAuthentication(httpRequestMessage);

        var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<T> PostAsync<T>(string url, IBaseRequest request,
        CancellationToken cancellationToken) where T : class
    {
        var response = await InternalPostAsync(url, request, cancellationToken);

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
            ?? throw new ArgumentNullException("Response must be returned", nameof(response.Content));
    }

    public async Task PostAsync(string url, IBaseRequest request, CancellationToken cancellationToken)
    {
        await InternalPostAsync(url, request, cancellationToken);
    }

    private async Task<HttpResponseMessage> InternalGetAsync(string url, CancellationToken cancellationToken)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        SetAuthentication(httpRequestMessage);

        var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken) where T : class
    {
        var response = await InternalGetAsync(url, cancellationToken);

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
            ?? throw new ArgumentNullException("Response must be returned", nameof(response.Content));
    }

    private async Task<HttpResponseMessage> InternalPutAsync(string url, IBaseRequest? request,
        CancellationToken cancellationToken)
    {
        HttpRequestMessage httpRequestMessage;

        if (request != null)
        {
            var json = JsonSerializer.Serialize(request, request.GetType(), JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url) { Content = content };
        }
        else
        {
            httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, url);
        }

        SetAuthentication(httpRequestMessage);

        var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<T> PutAsync<T>(string url, IBaseRequest? request, CancellationToken cancellationToken) where T : class
    {
        var response = await InternalPutAsync(url, request, cancellationToken);

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
            ?? throw new ArgumentNullException("Response must be returned", nameof(response.Content));
    }

    public async Task PutAsync(string url, IBaseRequest? request, CancellationToken cancellationToken)
    {
        await InternalPutAsync(url, request, cancellationToken);
    }

    private async Task<HttpResponseMessage> InternalDeleteAsync(string url, CancellationToken cancellationToken)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
        SetAuthentication(httpRequestMessage);

        var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<T> DeleteAsync<T>(string url, CancellationToken cancellationToken) where T : class
    {
        var response = await InternalDeleteAsync(url, cancellationToken);

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
            ?? throw new ArgumentNullException("Response must be returned", nameof(response.Content));
    }

    public async Task DeleteAsync(string url, CancellationToken cancellationToken)
    {
        await InternalDeleteAsync(url, cancellationToken);
    }

    private async Task<HttpResponseMessage> InternalPatchAsync(string url, IBaseRequest? request,
        CancellationToken cancellationToken)
    {
        HttpRequestMessage httpRequestMessage;

        if (request != null)
        {
            var json = JsonSerializer.Serialize(request, request.GetType(), JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, url) { Content = content };
        }
        else
        {
            httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, url);
        }

        SetAuthentication(httpRequestMessage);

        var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<T> PatchAsync<T>(string url, IBaseRequest? request, CancellationToken cancellationToken) where T : class
    {
        var response = await InternalPatchAsync(url, request, cancellationToken);

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions, cancellationToken)
            ?? throw new ArgumentNullException("Response must be returned", nameof(response.Content));
    }

    public async Task PatchAsync(string url, IBaseRequest? request, CancellationToken cancellationToken)
    {
        await InternalPatchAsync(url, request, cancellationToken);
    }
}
