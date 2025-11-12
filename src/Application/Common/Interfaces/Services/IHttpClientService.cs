using MediatR;

namespace Application.Common.Interfaces.Services;

public interface IHttpClientService
{
    Task<T> PostAsync<T>(string url, IBaseRequest request, CancellationToken cancellationToken) where T : class;
    Task PostAsync(string url, IBaseRequest request, CancellationToken cancellationToken);
    Task<T> GetAsync<T>(string url, CancellationToken cancellationToken) where T : class;
    Task<T> PutAsync<T>(string url, IBaseRequest? request, CancellationToken cancellationToken) where T : class;
    Task PutAsync(string url, IBaseRequest? request, CancellationToken cancellationToken);
    Task<T> DeleteAsync<T>(string url, CancellationToken cancellationToken) where T : class;
    Task DeleteAsync(string url, CancellationToken cancellationToken);
    Task<T> PatchAsync<T>(string url, IBaseRequest? request, CancellationToken cancellationToken) where T : class;
    Task PatchAsync(string url, IBaseRequest? request, CancellationToken cancellationToken);
}

