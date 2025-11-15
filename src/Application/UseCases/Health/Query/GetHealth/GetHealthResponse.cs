namespace Application.UseCases.Health.Query.GetHealth;

public class GetHealthResponse
{
    public string Status { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}
