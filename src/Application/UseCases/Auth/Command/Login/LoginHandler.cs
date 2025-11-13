using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Auth.Command.Login;

public class LoginHandler(IInternalApiService internalApiService)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await internalApiService.LoginAsync(request, cancellationToken);
    }
}
