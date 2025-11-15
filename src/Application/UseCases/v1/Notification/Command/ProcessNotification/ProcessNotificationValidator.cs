using FluentValidation;

namespace Application.UseCases.v1.Notification.Command.ProcessNotification;

public class ProcessNotificationValidator : AbstractValidator<ProcessNotificationCommand>
{
    public ProcessNotificationValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.EventName)
            .NotEmpty();

        RuleFor(x => x.Data)
            .NotNull();

        RuleFor(x => x.CreatedAt)
            .NotEmpty();
    }
}


