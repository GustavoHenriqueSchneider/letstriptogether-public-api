using Application.Common.Interfaces.Services;
using Infrastructure.Configurations;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationExternalDependencies(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(configuration);
        return services;
    }
    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IHttpClientService, HttpClientService>((_, client) =>
        {
            var internalApiSettings = configuration
                .GetRequiredSection(nameof(InternalApiSettings))
                .Get<InternalApiSettings>()!;

            client.BaseAddress = new Uri(internalApiSettings.BaseAddress);
        });

        services.AddTransient<IInternalApiService, InternalApiService>();
    }
}
