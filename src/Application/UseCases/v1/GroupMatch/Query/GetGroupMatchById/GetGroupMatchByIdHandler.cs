using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.GroupMatch.Query.GetGroupMatchById;

public class GetGroupMatchByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetGroupMatchByIdQuery, GetGroupMatchByIdResponse>
{
    public async Task<GetGroupMatchByIdResponse> Handle(GetGroupMatchByIdQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetGroupMatchByIdAsync(request, cancellationToken);
    }
}

