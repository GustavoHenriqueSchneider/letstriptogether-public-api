using MediatR;

namespace Application.UseCases.v1.GroupInvitation.Command.CreateGroupInvitation;

public class CreateGroupInvitationCommand : IRequest<CreateGroupInvitationResponse>
{
    public Guid GroupId { get; init; }
}

