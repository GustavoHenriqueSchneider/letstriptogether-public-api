using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.v1.User.Command.ChangeCurrentUserPassword;

public class ChangeCurrentUserPasswordHandler(IInternalApiService internalApiService)
    : IRequestHandler<ChangeCurrentUserPasswordCommand>
{
    public async Task Handle(ChangeCurrentUserPasswordCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.ChangeCurrentUserPasswordAsync(request, cancellationToken);
    }
}
