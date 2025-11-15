using MediatR;

namespace Application.UseCases.v1.GroupInvitation.Command.CancelActiveGroupInvitation;

public class CancelActiveGroupInvitationCommand : IRequest
{
    public Guid GroupId { get; init; }
}

