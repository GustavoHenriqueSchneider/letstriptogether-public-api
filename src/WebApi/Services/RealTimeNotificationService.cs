using Application.Common.Interfaces.Services;
using Domain.Common;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace WebApi.Services;

public class RealTimeNotificationService(
    IHubContext<NotificationHub> hubContext,
    ILogger<RealTimeNotificationService> logger) : IRealTimeNotificationService
{
    public async Task SendToUserAsync(Guid userId, RealTimeNotification payload,
        CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
        {
            logger.LogWarning("Attempted to send notification without user id. Payload {@Payload}", payload);
            return;
        }

        var notification = new
        {
            id = payload.Id,
            type = payload.Type,
            title = payload.Title,
            message = payload.Message,
            createdAt = payload.CreatedAt.ToString("O"),
            read = false
        };

        try
        {
            await hubContext
                .Clients
                .Group($"user_{userId}")
                .SendAsync("ReceiveNotification", notification, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error while sending realtime notification to user {UserId}. Payload {@Payload}",
                userId, payload);
        }
    }
}

