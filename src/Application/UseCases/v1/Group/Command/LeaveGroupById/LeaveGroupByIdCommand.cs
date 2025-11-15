using MediatR;

namespace Application.UseCases.v1.Group.Command.LeaveGroupById;

public class LeaveGroupByIdCommand : IRequest
{
    public Guid GroupId { get; init; }
}

