namespace Infrastructure.Events.GroupMatchCreated;

public class GroupMatchCreatedEventData
{
    public Guid GroupId { get; init; }
    public Guid DestinationId { get; init; }
}
