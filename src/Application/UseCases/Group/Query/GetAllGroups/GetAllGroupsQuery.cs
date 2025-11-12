using System.Text.Json.Serialization;
using MediatR;

namespace Application.UseCases.Group.Query.GetAllGroups;

public class GetAllGroupsQuery : IRequest<GetAllGroupsResponse>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

