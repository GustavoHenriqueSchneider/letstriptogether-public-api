using System.Text.Json.Serialization;
using MediatR;

namespace Application.UseCases.Group.Command.DeleteGroupById;

public class DeleteGroupByIdCommand : IRequest
{
    public Guid GroupId { get; init; }
}

