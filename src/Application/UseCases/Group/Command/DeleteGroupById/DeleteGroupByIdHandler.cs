using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Group.Command.DeleteGroupById;

public class DeleteGroupByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<DeleteGroupByIdCommand>
{
    public async Task Handle(DeleteGroupByIdCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.DeleteGroupByIdAsync(request, cancellationToken);
    }
}

