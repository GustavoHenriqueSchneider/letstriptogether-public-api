using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.GroupInvitation.Query.GetActiveGroupInvitation;

public class GetActiveGroupInvitationHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetActiveGroupInvitationQuery, GetActiveGroupInvitationResponse>
{
    public async Task<GetActiveGroupInvitationResponse> Handle(GetActiveGroupInvitationQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetActiveGroupInvitationAsync(request, cancellationToken);
    }
}

