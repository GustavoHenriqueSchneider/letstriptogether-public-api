using MediatR;

namespace Application.UseCases.Invitation.Command.RefuseInvitation;

public record RefuseInvitationCommand : IRequest
{
    public string Token { get; init; } = null!;
}

