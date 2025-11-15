using MediatR;

namespace Application.UseCases.v1.GroupDestinationVote.Query.GetGroupDestinationVoteById;

public class GetGroupDestinationVoteByIdQuery : IRequest<GetGroupDestinationVoteByIdResponse>
{
    public Guid GroupId { get; init; }
    public Guid DestinationVoteId { get; init; }
}

