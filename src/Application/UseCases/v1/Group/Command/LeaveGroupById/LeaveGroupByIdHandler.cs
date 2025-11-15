using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.Group.Command.LeaveGroupById;

public class LeaveGroupByIdHandler(IInternalApiService internalApiService)
    : IRequestHandler<LeaveGroupByIdCommand>
{
    public async Task Handle(LeaveGroupByIdCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.LeaveGroupByIdAsync(request, cancellationToken);
    }
}

