using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.Group.Query.GetAllGroups;

public class GetAllGroupsHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetAllGroupsQuery, GetAllGroupsResponse>
{
    public async Task<GetAllGroupsResponse> Handle(GetAllGroupsQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetAllGroupsAsync(request, cancellationToken);
    }
}

