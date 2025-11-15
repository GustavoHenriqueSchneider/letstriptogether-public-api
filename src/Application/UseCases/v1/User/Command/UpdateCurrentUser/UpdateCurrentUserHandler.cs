using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.User.Command.UpdateCurrentUser;

public class UpdateCurrentUserHandler(IInternalApiService internalApiService)
    : IRequestHandler<UpdateCurrentUserCommand>
{
    public async Task Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.UpdateCurrentUserAsync(request, cancellationToken);
    }
}

