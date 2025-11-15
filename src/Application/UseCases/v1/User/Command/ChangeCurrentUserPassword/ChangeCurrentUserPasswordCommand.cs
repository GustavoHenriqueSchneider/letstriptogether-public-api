using MediatR;

namespace Application.UseCases.v1.User.Command.ChangeCurrentUserPassword;

public record ChangeCurrentUserPasswordCommand : IRequest
{
    public string CurrentPassword { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}
