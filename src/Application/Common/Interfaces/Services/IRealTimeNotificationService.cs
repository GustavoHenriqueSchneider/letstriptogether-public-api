using Domain.Common;

namespace Application.Common.Interfaces.Services;

public interface IRealTimeNotificationService
{
    Task SendToUserAsync(Guid userId, RealTimeNotification payload, 
        CancellationToken cancellationToken = default);
}
