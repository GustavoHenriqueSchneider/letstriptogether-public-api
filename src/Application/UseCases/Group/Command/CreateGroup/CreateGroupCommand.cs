using System.Text.Json.Serialization;
using MediatR;

namespace Application.UseCases.Group.Command.CreateGroup;

public record CreateGroupCommand : IRequest<CreateGroupResponse>
{
    public string Name { get; init; } = null!;
    public DateTime TripExpectedDate { get; init; }
}

