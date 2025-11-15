using Application.Common.Interfaces.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks;

public class InternalApiHealthCheck(IInternalApiService internalApiService) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var healthResponse = await internalApiService.GetHealthAsync(cancellationToken);
            
            return HealthCheckResult.Healthy(
                $"Internal API is healthy. Status: {healthResponse.Status}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Internal API health check failed", ex);
        }
    }
}

