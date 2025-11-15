using MediatR;

namespace Application.UseCases.v1.Invitation.Query.GetInvitation;

public class GetInvitationQuery : IRequest<GetInvitationResponse>
{
    public string Token { get; init; } = null!;
}
