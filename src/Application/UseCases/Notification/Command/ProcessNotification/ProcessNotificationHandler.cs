using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Notification.Command.ProcessNotification;

public class ProcessNotificationHandler : IRequestHandler<ProcessNotificationCommand>
{
    private readonly IEnumerable<INotificationEventHandler> _eventHandlers;
    private readonly ILogger<ProcessNotificationHandler> _logger;

    public ProcessNotificationHandler(
        IEnumerable<INotificationEventHandler> eventHandlers,
        ILogger<ProcessNotificationHandler> logger)
    {
        _eventHandlers = eventHandlers;
        _logger = logger;
    }

    public async Task Handle(ProcessNotificationCommand request, CancellationToken cancellationToken)
    {
        var handler = _eventHandlers.FirstOrDefault(h => h.CanHandle(request.EventName));

        if (handler is null)
        {
            _logger.LogWarning(
                "No handler found for event type: {EventName}. Event will be ignored.",
                request.EventName);
            return;
        }

        try
        {
            await handler.HandleAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error processing notification event {EventName} for user {UserId}",
                request.EventName,
                request.UserId);
            throw;
        }
    }
}

