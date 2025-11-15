using MediatR;

namespace Application.UseCases.v1.Group.Command.CreateGroup;

public record CreateGroupCommand : IRequest<CreateGroupResponse>
{
    public string Name { get; init; } = null!;
    public DateTime TripExpectedDate { get; init; }
}

