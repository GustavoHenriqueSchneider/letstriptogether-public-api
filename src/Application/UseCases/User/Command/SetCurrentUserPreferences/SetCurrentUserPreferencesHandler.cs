using Application.Common.Interfaces.Services;
using MediatR;

namespace Application.UseCases.User.Command.SetCurrentUserPreferences;

public class SetCurrentUserPreferencesHandler(IInternalApiService internalApiService)
    : IRequestHandler<SetCurrentUserPreferencesCommand>
{
    public async Task Handle(SetCurrentUserPreferencesCommand request, CancellationToken cancellationToken)
    {
        await internalApiService.SetCurrentUserPreferencesAsync(request, cancellationToken);
    }
}

