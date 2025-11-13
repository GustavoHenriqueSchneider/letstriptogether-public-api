using System.Text.Json;
using Application.Common.Interfaces;
using Application.UseCases.Notification.Command.ProcessNotification;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Events.GroupMatchCreated;

public class GroupMatchCreatedEventHandler : INotificationEventHandler
{
    private readonly ILogger<GroupMatchCreatedEventHandler> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public GroupMatchCreatedEventHandler(ILogger<GroupMatchCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public bool CanHandle(string eventName)
    {
        return eventName == NotificationEvents.GroupMatchCreated;
    }

    public async Task HandleAsync(ProcessNotificationCommand command, CancellationToken cancellationToken)
    {
        var eventData = DeserializeEventData(command.Data);
        
        if (eventData is null)
        {
            return;
        }

        // TODO: Implementar lógica de processamento da notificação
        // Por exemplo: enviar push notification, email, salvar no banco, etc.
        
        await Task.CompletedTask;
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

