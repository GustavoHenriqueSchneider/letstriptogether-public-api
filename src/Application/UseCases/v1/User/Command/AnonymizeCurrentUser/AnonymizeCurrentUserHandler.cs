using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.User.Command.AnonymizeCurrentUser;

public class AnonymizeCurrentUserHandler(IInternalApiService internalApiService)
    : IRequestHandler<AnonymizeCurrentUserCommand>
{
    public async Task Handle(AnonymizeCurrentUserCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.AnonymizeCurrentUserAsync(request, cancellationToken);
    }
}

