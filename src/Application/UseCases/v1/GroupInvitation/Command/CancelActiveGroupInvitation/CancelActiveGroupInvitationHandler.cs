using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.GroupInvitation.Command.CancelActiveGroupInvitation;

public class CancelActiveGroupInvitationHandler(IInternalApiService internalApiService)
    : IRequestHandler<CancelActiveGroupInvitationCommand>
{
    public async Task Handle(CancelActiveGroupInvitationCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.CancelActiveGroupInvitationAsync(request, cancellationToken);
    }
}

