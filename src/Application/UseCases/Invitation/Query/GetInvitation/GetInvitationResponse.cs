namespace Application.UseCases.Invitation.Query.GetInvitation;

public class GetInvitationResponse
{
    public string CreatedBy { get; init; } = null!;
    public string GroupName { get; init; } = null!;
    public bool IsActive { get; init; }
}
