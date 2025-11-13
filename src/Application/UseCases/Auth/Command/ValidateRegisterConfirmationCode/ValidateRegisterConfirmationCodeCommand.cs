using MediatR;

namespace Application.UseCases.Auth.Command.ValidateRegisterConfirmationCode;

public record ValidateRegisterConfirmationCodeCommand : IRequest<ValidateRegisterConfirmationCodeResponse>
{
    public int Code { get; init; }
}
