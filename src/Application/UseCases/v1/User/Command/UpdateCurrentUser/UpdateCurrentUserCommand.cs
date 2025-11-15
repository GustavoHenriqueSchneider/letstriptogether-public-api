using MediatR;

namespace Application.UseCases.v1.User.Command.UpdateCurrentUser;

public record UpdateCurrentUserCommand : IRequest
{
    public string Name { get; init; } = null!;
}

