using System.Text;
using Application.Common.Interfaces.Services;
using Infrastructure.Configurations;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationExternalDependencies(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(configuration);
        return services;
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
