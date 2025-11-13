using MediatR;

namespace Application.UseCases.GroupInvitation.Query.GetActiveGroupInvitation;

public class GetActiveGroupInvitationQuery : IRequest<GetActiveGroupInvitationResponse>
{
    public Guid GroupId { get; init; }
}

