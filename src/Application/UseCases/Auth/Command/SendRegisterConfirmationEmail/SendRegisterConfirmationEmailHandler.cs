using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Auth.Command.SendRegisterConfirmationEmail;

public class SendRegisterConfirmationEmailHandler(IInternalApiService internalApiService)
    : IRequestHandler<SendRegisterConfirmationEmailCommand, SendRegisterConfirmationEmailResponse>
{
    public async Task<SendRegisterConfirmationEmailResponse> Handle(SendRegisterConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        return await internalApiService.SendRegisterConfirmationEmailAsync(request, cancellationToken);
    }
}
