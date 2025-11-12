using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Auth.Command.RefreshToken;

public class RefreshTokenHandler(IInternalApiService internalApiService)
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await internalApiService.RefreshTokenAsync(request, cancellationToken);
    }
}
