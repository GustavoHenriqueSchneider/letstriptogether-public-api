using Infrastructure.Configurations;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Services.Interfaces;

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
        services.AddHttpClient<IInternalApiService, InternalApiService>((_, client) =>
        {
            var internalApiSettings = configuration
                .GetRequiredSection(nameof(InternalApiSettings))
                .Get<InternalApiSettings>()!;

            client.BaseAddress = new Uri(internalApiSettings.BaseAddress);
        });
    }
}
