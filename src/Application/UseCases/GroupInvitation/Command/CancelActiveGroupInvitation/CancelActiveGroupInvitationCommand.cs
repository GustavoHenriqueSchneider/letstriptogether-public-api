using MediatR;

namespace Application.UseCases.GroupInvitation.Command.CancelActiveGroupInvitation;

public class CancelActiveGroupInvitationCommand : IRequest
{
    public Guid GroupId { get; init; }
}

