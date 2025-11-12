using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.GroupMember.Command.RemoveGroupMemberById;

public class RemoveGroupMemberByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<RemoveGroupMemberByIdCommand>
{
    public async Task Handle(RemoveGroupMemberByIdCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.RemoveGroupMemberByIdAsync(request, cancellationToken);
    }
}

