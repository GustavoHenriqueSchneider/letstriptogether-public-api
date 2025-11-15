using MediatR;

namespace Application.UseCases.v1.Invitation.Command.AcceptInvitation;

public record AcceptInvitationCommand : IRequest
{
    public string Token { get; init; } = null!;
}

