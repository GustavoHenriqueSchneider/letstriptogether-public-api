using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.GroupDestinationVote.Command.UpdateDestinationVoteById;

public class UpdateDestinationVoteByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<UpdateDestinationVoteByIdCommand>
{
    public async Task Handle(UpdateDestinationVoteByIdCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.UpdateDestinationVoteByIdAsync(request, cancellationToken);
    }
}

