using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.Group.Query.GetGroupById;

public class GetGroupByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetGroupByIdQuery, GetGroupByIdResponse>
{
    public async Task<GetGroupByIdResponse> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetGroupByIdAsync(request, cancellationToken);
    }
}

