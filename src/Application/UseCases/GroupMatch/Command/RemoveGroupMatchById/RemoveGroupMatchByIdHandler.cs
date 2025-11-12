using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.GroupMatch.Command.RemoveGroupMatchById;

public class RemoveGroupMatchByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<RemoveGroupMatchByIdCommand>
{
    public async Task Handle(RemoveGroupMatchByIdCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.RemoveGroupMatchByIdAsync(request, cancellationToken);
    }
}

