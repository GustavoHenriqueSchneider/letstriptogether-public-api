using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.GroupMatch.Query.GetAllGroupMatchesById;

public class GetAllGroupMatchesByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetAllGroupMatchesByIdQuery, GetAllGroupMatchesByIdResponse>
{
    public async Task<GetAllGroupMatchesByIdResponse> Handle(GetAllGroupMatchesByIdQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetAllGroupMatchesByIdAsync(request, cancellationToken);
    }
}

