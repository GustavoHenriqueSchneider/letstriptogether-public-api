using MediatR;

namespace Application.UseCases.v1.Auth.Command.Register;

public record RegisterCommand : IRequest<RegisterResponse>
{
    public string Password { get; init; } = null!;
    public bool HasAcceptedTermsOfUse { get; init; }
}
