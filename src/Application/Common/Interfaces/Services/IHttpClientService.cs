using MediatR;

namespace Application.Common.Interfaces.Services;

public interface IHttpClientService
{
    Task<T> PostAsync<T>(string url, IBaseRequest request, CancellationToken cancellationToken) where T : class;
    Task PostAsync(string url, IBaseRequest request, CancellationToken cancellationToken);
    Task<T> GetAsync<T>(string url, CancellationToken cancellationToken) where T : class;
    Task PutAsync(string url, IBaseRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(string url, CancellationToken cancellationToken);
    Task PatchAsync(string url, IBaseRequest request, CancellationToken cancellationToken);
}

