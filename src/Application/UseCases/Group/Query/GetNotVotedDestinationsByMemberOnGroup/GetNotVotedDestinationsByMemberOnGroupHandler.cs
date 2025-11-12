using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Group.Query.GetNotVotedDestinationsByMemberOnGroup;

public class GetNotVotedDestinationsByMemberOnGroupHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetNotVotedDestinationsByMemberOnGroupQuery, GetNotVotedDestinationsByMemberOnGroupResponse>
{
    public async Task<GetNotVotedDestinationsByMemberOnGroupResponse> Handle(GetNotVotedDestinationsByMemberOnGroupQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetNotVotedDestinationsByMemberOnGroupAsync(request, cancellationToken);
    }
}

