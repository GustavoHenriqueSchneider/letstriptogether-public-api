using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.User.Query.GetCurrentUser;

public class GetCurrentUserHandler(IInternalApiService internalApiService)
    : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        return await internalApiService.GetCurrentUserAsync(request, cancellationToken);
    }
}

