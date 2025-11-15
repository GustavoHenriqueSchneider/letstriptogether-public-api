using MediatR;

namespace Application.UseCases.v1.Group.Command.DeleteGroupById;

public class DeleteGroupByIdCommand : IRequest
{
    public Guid GroupId { get; init; }
}

