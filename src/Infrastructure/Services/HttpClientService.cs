using System.Net.Http.Json;
using Application.Common.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

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

        var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            httpRequestMessage.Headers.Add("Authorization", authorizationHeader);
        }
    }

    private async Task<HttpResponseMessage> InternalPostAsync(string url, IBaseRequest request,
        CancellationToken cancellationToken)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(request)
        };

        SetAuthentication(httpRequestMessage);

        var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        return response;
    }

    public async Task<T> PostAsync<T>(string url, IBaseRequest request,
        CancellationToken cancellationToken) where T : class
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(request)
        };

        SetAuthentication(httpRequestMessage);

        var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
            ?? throw new ArgumentNullException("Response must be returned", nameof(response.Content));
    }

    public async Task PostAsync(string url, IBaseRequest request, CancellationToken cancellationToken)
    {
        await InternalPostAsync(url, request, cancellationToken);
    }
}
