using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.Group.Command.CreateGroup;

public class CreateGroupHandler(IInternalApiService internalApiService)
    : IRequestHandler<CreateGroupCommand, CreateGroupResponse>
{
    public async Task<CreateGroupResponse> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        return await internalApiService.CreateGroupAsync(request, cancellationToken);
    }
}

