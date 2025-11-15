using System.Text;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Infrastructure.Configurations;
using Infrastructure.Events.GroupMatchCreated;
using Infrastructure.HealthChecks;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationExternalDependencies(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(configuration);
        services.RegisterHealthChecks(configuration);
        return services;
    }
    
    private static void RegisterHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddCheck<InternalApiHealthCheck>(
                "internal_api",
                tags: new[] { "internal", "api" });
    }

    public static void RegisterApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration
            .GetRequiredSection(nameof(JsonWebTokenSettings))
            .Get<JsonWebTokenSettings>()!;

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IHttpClientService, HttpClientService>((serviceProvider, client) =>
        {
            var internalApiSettings = configuration
                .GetRequiredSection(nameof(InternalApiSettings))
                .Get<InternalApiSettings>()!;

            client.BaseAddress = new Uri(internalApiSettings.BaseAddress);
        });

        services.AddTransient<IInternalApiService, InternalApiService>();
        services.RegisterNotificationEventHandlers();
    }

    private static void RegisterNotificationEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<INotificationEventHandler, GroupMatchCreatedEventHandler>();
    }
}
