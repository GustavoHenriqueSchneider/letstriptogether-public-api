using MediatR;

namespace Application.UseCases.User.Command.ChangeCurrentUserPassword;

public record ChangeCurrentUserPasswordCommand : IRequest
{
    public string CurrentPassword { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}
