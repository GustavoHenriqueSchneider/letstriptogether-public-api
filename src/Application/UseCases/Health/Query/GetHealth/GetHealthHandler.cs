using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Application.UseCases.Health.Query.GetHealth;

public class GetHealthHandler(HealthCheckService healthCheckService)
    : IRequestHandler<GetHealthQuery, GetHealthResponse>
{
    public async Task<GetHealthResponse> Handle(GetHealthQuery request, CancellationToken cancellationToken)
    {
        var healthReport = await healthCheckService.CheckHealthAsync(cancellationToken);

        return new GetHealthResponse
        {
            Status = healthReport.Status == HealthStatus.Healthy 
                ? nameof(HealthStatus.Healthy).ToLowerInvariant() 
                : nameof(HealthStatus.Unhealthy).ToLowerInvariant(),
            Timestamp = DateTime.UtcNow
        };
    }
}