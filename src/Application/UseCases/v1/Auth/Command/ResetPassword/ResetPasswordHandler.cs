using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.Auth.Command.ResetPassword;

public class ResetPasswordHandler(IInternalApiService internalApiService)
    : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.ResetPasswordAsync(request, cancellationToken);
    }
}
