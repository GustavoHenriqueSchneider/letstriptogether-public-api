using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.Auth.Command.Logout;

public class LogoutHandler(IInternalApiService internalApiService)
    : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.LogoutAsync(request, cancellationToken);
    }
}
