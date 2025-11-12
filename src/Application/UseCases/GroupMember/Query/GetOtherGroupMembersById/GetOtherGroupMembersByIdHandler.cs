using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.GroupMember.Query.GetOtherGroupMembersById;

public class GetOtherGroupMembersByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetOtherGroupMembersByIdQuery, GetOtherGroupMembersByIdResponse>
{
    public async Task<GetOtherGroupMembersByIdResponse> Handle(GetOtherGroupMembersByIdQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetOtherGroupMembersByIdAsync(request, cancellationToken);
    }
}

