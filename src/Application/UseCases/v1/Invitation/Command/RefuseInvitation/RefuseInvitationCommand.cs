using MediatR;

namespace Application.UseCases.v1.Invitation.Command.RefuseInvitation;

public record RefuseInvitationCommand : IRequest
{
    public string Token { get; init; } = null!;
}

