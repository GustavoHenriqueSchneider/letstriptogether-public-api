using MediatR;

namespace Application.UseCases.v1.Notification.Command.ProcessNotification;

public record ProcessNotificationCommand : IRequest
{
    public Guid UserId { get; init; }
    public string EventName { get; init; } = null!;
    public object Data { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}


