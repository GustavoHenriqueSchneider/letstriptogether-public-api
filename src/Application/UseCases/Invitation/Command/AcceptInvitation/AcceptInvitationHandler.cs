using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Invitation.Command.AcceptInvitation;

public class AcceptInvitationHandler(IInternalApiService internalApiService)
    : IRequestHandler<AcceptInvitationCommand>
{
    public async Task Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.AcceptInvitationAsync(request, cancellationToken);
    }
}

