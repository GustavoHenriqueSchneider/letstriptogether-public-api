using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Auth.Command.RequestResetPassword;

public class RequestResetPasswordHandler(IInternalApiService internalApiService)
    : IRequestHandler<RequestResetPasswordCommand>
{
    public async Task Handle(RequestResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.RequestResetPasswordAsync(request, cancellationToken);
    }
}
