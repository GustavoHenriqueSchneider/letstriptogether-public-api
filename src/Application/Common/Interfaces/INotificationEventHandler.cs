using Application.UseCases.v1.Notification.Command.ProcessNotification;

namespace Application.Common.Interfaces;

public interface INotificationEventHandler
{
    bool CanHandle(string eventName);
    Task HandleAsync(ProcessNotificationCommand command, CancellationToken cancellationToken);
}


