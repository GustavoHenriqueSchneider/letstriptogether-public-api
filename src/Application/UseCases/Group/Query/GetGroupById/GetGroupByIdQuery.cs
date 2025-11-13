using MediatR;

namespace Application.UseCases.Group.Query.GetGroupById;

public class GetGroupByIdQuery : IRequest<GetGroupByIdResponse>
{
    public Guid GroupId { get; init; }
}

