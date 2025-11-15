using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.Destination.Query.GetDestinationById;

public class GetDestinationByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetDestinationByIdQuery, GetDestinationByIdResponse>
{
    public async Task<GetDestinationByIdResponse> Handle(GetDestinationByIdQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetDestinationByIdAsync(request, cancellationToken);
    }
}

