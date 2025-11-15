using MediatR;

namespace Application.UseCases.v1.User.Command.SetCurrentUserPreferences;

public record SetCurrentUserPreferencesCommand : IRequest
{
    public bool LikesShopping { get; init; }
    public bool LikesGastronomy { get; init; }
    public List<string> Culture { get; init; } = [];
    public List<string> Entertainment { get; init; } = [];
    public List<string> PlaceTypes { get; init; } = [];
}

