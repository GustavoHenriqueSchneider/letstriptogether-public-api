namespace Domain.Common;

public record RealTimeNotification
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Type { get; init; } = "match";
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}