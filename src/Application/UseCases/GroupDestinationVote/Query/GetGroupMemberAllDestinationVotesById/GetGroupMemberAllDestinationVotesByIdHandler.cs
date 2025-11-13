using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.GroupDestinationVote.Query.GetGroupMemberAllDestinationVotesById;

public class GetGroupMemberAllDestinationVotesByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetGroupMemberAllDestinationVotesByIdQuery, GetGroupMemberAllDestinationVotesByIdResponse>
{
    public async Task<GetGroupMemberAllDestinationVotesByIdResponse> Handle(GetGroupMemberAllDestinationVotesByIdQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetGroupMemberAllDestinationVotesByIdAsync(request, cancellationToken);
    }
}

