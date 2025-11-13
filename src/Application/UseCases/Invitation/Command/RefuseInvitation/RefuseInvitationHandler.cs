using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Invitation.Command.RefuseInvitation;

public class RefuseInvitationHandler(IInternalApiService internalApiService)
    : IRequestHandler<RefuseInvitationCommand>
{
    public async Task Handle(RefuseInvitationCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.RefuseInvitationAsync(request, cancellationToken);
    }
}

