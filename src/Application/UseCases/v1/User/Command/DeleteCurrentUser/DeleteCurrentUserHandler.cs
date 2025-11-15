using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.User.Command.DeleteCurrentUser;

public class DeleteCurrentUserHandler(IInternalApiService internalApiService)
    : IRequestHandler<DeleteCurrentUserCommand>
{
    public async Task Handle(DeleteCurrentUserCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.DeleteCurrentUserAsync(request, cancellationToken);
    }
}

