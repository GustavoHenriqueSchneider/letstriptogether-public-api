using System.Net.Http;


namespace PublicApi.Services.Interfaces;

public interface IInternalApiService
{
    Task<HttpResponseMessage> GetAsync(string endpoint, string? token = null);
    
    Task<HttpResponseMessage> PostAsync(string endpoint, object? body, string? token = null);
    
    Task<HttpResponseMessage> PutAsync(string endpoint, object? body, string? token = null);
    
    Task<HttpResponseMessage> DeleteAsync(string endpoint, string? token = null);
    
    Task<HttpResponseMessage> PatchAsync(string endpoint, object? body, string? token = null);
}

