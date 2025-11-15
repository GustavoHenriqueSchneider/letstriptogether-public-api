using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.Invitation.Query.GetInvitation;

public class GetInvitationHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetInvitationQuery, GetInvitationResponse>
{
    public async Task<GetInvitationResponse> Handle(GetInvitationQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetInvitationAsync(request, cancellationToken);
    }
}
