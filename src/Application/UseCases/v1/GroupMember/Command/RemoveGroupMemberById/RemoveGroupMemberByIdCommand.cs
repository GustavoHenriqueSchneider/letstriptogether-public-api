using MediatR;

namespace Application.UseCases.v1.GroupMember.Command.RemoveGroupMemberById;

public class RemoveGroupMemberByIdCommand : IRequest
{
    public Guid GroupId { get; init; }
    public Guid MemberId { get; init; }
}

