using System.Text.Json.Serialization;
using MediatR;

namespace Application.UseCases.Auth.Command.ValidateRegisterConfirmationCode;

public record ValidateRegisterConfirmationCodeCommand : IRequest<ValidateRegisterConfirmationCodeResponse>
{
    public string Code { get; init; } = null!;
}
