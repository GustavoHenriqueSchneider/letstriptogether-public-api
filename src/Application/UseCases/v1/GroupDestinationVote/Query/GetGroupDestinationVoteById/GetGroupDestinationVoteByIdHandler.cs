using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.GroupDestinationVote.Query.GetGroupDestinationVoteById;

public class GetGroupDestinationVoteByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetGroupDestinationVoteByIdQuery, GetGroupDestinationVoteByIdResponse>
{
    public async Task<GetGroupDestinationVoteByIdResponse> Handle(GetGroupDestinationVoteByIdQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetGroupDestinationVoteByIdAsync(request, cancellationToken);
    }
}

