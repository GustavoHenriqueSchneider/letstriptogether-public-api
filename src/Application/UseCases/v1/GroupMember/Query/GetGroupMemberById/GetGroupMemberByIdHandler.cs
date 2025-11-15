using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.GroupMember.Query.GetGroupMemberById;

public class GetGroupMemberByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetGroupMemberByIdQuery, GetGroupMemberByIdResponse>
{
    public async Task<GetGroupMemberByIdResponse> Handle(GetGroupMemberByIdQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetGroupMemberByIdAsync(request, cancellationToken);
    }
}

