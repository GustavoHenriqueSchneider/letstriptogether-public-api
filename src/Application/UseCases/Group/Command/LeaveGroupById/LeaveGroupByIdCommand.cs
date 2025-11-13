using MediatR;

namespace Application.UseCases.Group.Command.LeaveGroupById;

public class LeaveGroupByIdCommand : IRequest
{
    public Guid GroupId { get; init; }
}

