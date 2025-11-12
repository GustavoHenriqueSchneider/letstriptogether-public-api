using System.Text.Json.Serialization;
using MediatR;

namespace Application.UseCases.User.Command.SetCurrentUserPreferences;

public record SetCurrentUserPreferencesCommand : IRequest
{
    public bool LikesCommercial { get; init; }
    public List<string> Food { get; init; } = [];
    public List<string> Culture { get; init; } = [];
    public List<string> Entertainment { get; init; } = [];
    public List<string> PlaceTypes { get; init; } = [];
}

