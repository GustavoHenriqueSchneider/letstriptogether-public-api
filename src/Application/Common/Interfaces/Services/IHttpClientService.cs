using MediatR;

namespace Application.Common.Interfaces.Services;

public interface IHttpClientService
{
    Task<T> PostAsync<T>(string url, IBaseRequest request, CancellationToken cancellationToken) where T : class;
    Task PostAsync(string url, IBaseRequest request, CancellationToken cancellationToken);
}

