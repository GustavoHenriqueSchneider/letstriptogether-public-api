using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.GroupDestinationVote.Command.VoteAtDestinationForGroupId;

public class VoteAtDestinationForGroupIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<VoteAtDestinationForGroupIdCommand, VoteAtDestinationForGroupIdResponse>
{
    public async Task<VoteAtDestinationForGroupIdResponse> Handle(VoteAtDestinationForGroupIdCommand request, CancellationToken cancellationToken)
    {
        return await internalApiService.VoteAtDestinationForGroupIdAsync(request, cancellationToken);
    }
}

