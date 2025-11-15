using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.Auth.Command.Register;

public class RegisterHandler(IInternalApiService internalApiService)
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await internalApiService.RegisterAsync(request, cancellationToken);
    }
}
