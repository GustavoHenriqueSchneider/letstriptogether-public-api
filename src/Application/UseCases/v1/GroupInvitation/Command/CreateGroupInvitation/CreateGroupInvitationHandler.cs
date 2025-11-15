using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.GroupInvitation.Command.CreateGroupInvitation;

public class CreateGroupInvitationHandler(IInternalApiService internalApiService)
    : IRequestHandler<CreateGroupInvitationCommand, CreateGroupInvitationResponse>
{
    public async Task<CreateGroupInvitationResponse> Handle(CreateGroupInvitationCommand request, CancellationToken cancellationToken)
    {
        return await internalApiService.CreateGroupInvitationAsync(request, cancellationToken);
    }
}

