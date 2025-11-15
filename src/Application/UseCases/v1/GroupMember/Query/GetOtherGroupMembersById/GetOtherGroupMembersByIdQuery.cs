using MediatR;

namespace Application.UseCases.v1.GroupMember.Query.GetOtherGroupMembersById;

public class GetOtherGroupMembersByIdQuery : IRequest<GetOtherGroupMembersByIdResponse>
{
    public Guid GroupId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

