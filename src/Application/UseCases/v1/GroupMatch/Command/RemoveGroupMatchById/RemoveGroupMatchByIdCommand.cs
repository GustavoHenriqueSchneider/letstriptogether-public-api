using MediatR;

namespace Application.UseCases.v1.GroupMatch.Command.RemoveGroupMatchById;

public class RemoveGroupMatchByIdCommand : IRequest
{
    public Guid GroupId { get; init; }
    public Guid MatchId { get; init; }
}

