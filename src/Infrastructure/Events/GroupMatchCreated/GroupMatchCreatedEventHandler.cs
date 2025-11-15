using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Services;
using Application.UseCases.Destination.Query.GetDestinationById;
using Application.UseCases.Group.Query.GetGroupById;
using Application.UseCases.Notification.Command.ProcessNotification;
using Domain.Common;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Events.GroupMatchCreated;

public class GroupMatchCreatedEventHandler(
    ILogger<GroupMatchCreatedEventHandler> logger,
    IRealTimeNotificationService realTimeNotificationService,
    IInternalApiService internalApiService) : INotificationEventHandler
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public bool CanHandle(string eventName)
    {
        return eventName == NotificationEvents.GroupMatchCreated;
    }

    public async Task HandleAsync(ProcessNotificationCommand command, CancellationToken cancellationToken)
    {
        var eventData = DeserializeEventData(command.Data);
        
        if (eventData is null)
        {
            logger.LogWarning("GroupMatchCreated event data could not be deserialized for user {UserId}", 
                command.UserId);
            return;
        }

        if (command.UserId == Guid.Empty)
        {
            logger.LogWarning(
                "GroupMatchCreated event received without user id. EventData {@EventData}", eventData);
            return;
        }

        GetGroupByIdResponse group;

        try
        {
            group = await internalApiService.GetGroupByIdAsync(
                new GetGroupByIdQuery { GroupId = eventData.GroupId }, cancellationToken);
        }
        catch
        {
            logger.LogWarning(
                "Group not found for GroupMatchCreated event. EventData {@EventData}", eventData);
            return;
        }

        GetDestinationByIdResponse destination;

        try
        {
            destination = await internalApiService.GetDestinationByIdAsync(
                new GetDestinationByIdQuery { DestinationId = eventData.DestinationId }, cancellationToken);
        }
        catch
        {
            logger.LogWarning(
                "Destination not found for GroupMatchCreated event. EventData {@EventData}",
                eventData);
            return;
        }

        var payload = new RealTimeNotification
        {
            Type = "match",
            Title = "Novo match!",
            Message = $"O grupo '{group.Name}' encontrou um destino perfeito: {destination.Place}",
            CreatedAt = command.CreatedAt
        };

        await realTimeNotificationService.SendToUserAsync(command.UserId, payload, cancellationToken);
    }

    private static GroupMatchCreatedEventData? DeserializeEventData(object data)
    {
        try
        {
            if (data is JsonElement jsonElement)
            {
                return JsonSerializer.Deserialize<GroupMatchCreatedEventData>(jsonElement.GetRawText(), JsonOptions);
            }

            var jsonString = JsonSerializer.Serialize(data, JsonOptions);
            return JsonSerializer.Deserialize<GroupMatchCreatedEventData>(jsonString, JsonOptions);
        }
        catch
        {
            return null;
        }
    }
}

