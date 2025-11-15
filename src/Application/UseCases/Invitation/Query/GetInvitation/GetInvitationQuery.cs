using MediatR;

namespace Application.UseCases.Invitation.Query.GetInvitation;

public class GetInvitationQuery : IRequest<GetInvitationResponse>
{
    public string Token { get; init; } = null!;
}
