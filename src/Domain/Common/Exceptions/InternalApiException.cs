namespace Domain.Common.Exceptions;

public class InternalApiException
{
    public string Title { get; init; } = null!;
    public int Status { get; init; }
    public string Detail { get; init; } = null!;
}
