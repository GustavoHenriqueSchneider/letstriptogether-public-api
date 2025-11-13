using MediatR;

namespace Application.UseCases.Invitation.Command.AcceptInvitation;

public record AcceptInvitationCommand : IRequest
{
    public string Token { get; init; } = null!;
}

