using System.Text.Json.Serialization;
using MediatR;

namespace Application.UseCases.User.Command.UpdateCurrentUser;

public record UpdateCurrentUserCommand : IRequest
{
    public string Name { get; init; } = null!;
}

