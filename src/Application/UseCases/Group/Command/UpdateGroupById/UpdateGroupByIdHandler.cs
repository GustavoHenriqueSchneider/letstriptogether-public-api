using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Group.Command.UpdateGroupById;

public class UpdateGroupByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<UpdateGroupByIdCommand>
{
    public async Task Handle(UpdateGroupByIdCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.UpdateGroupByIdAsync(request, cancellationToken);
    }
}

