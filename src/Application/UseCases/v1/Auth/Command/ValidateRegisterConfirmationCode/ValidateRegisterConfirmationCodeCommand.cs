using MediatR;

namespace Application.UseCases.v1.Auth.Command.ValidateRegisterConfirmationCode;

public record ValidateRegisterConfirmationCodeCommand : IRequest<ValidateRegisterConfirmationCodeResponse>
{
    public int Code { get; init; }
}
