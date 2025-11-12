using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.Auth.Command.ValidateRegisterConfirmationCode;

public class ValidateRegisterConfirmationCodeHandler(IInternalApiService internalApiService)
    : IRequestHandler<ValidateRegisterConfirmationCodeCommand, ValidateRegisterConfirmationCodeResponse>
{
    public async Task<ValidateRegisterConfirmationCodeResponse> Handle(ValidateRegisterConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        return await internalApiService.ValidateRegisterConfirmationCodeAsync(request, cancellationToken);
    }
}
