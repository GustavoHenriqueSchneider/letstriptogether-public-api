using Application.Common.Extensions;
using Application.Common.Interfaces.Extensions;
using Domain.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    private readonly ILogger<NotificationHub> _logger;
    private IApplicationUserContextExtensions? _currentUser;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    private IApplicationUserContextExtensions GetCurrentUser()
    {
        return _currentUser ??= new ApplicationUserContextExtensions(Context?.User 
        ?? throw new InvalidOperationException("HttpContext or User is not available"));
    }

    private bool IsValidTokenType()
    {
        var currentUser = GetCurrentUser();
        var tokenType = currentUser.GetTokenType();
        return tokenType == TokenTypes.Access || tokenType == TokenTypes.Refresh;
    }

    public override async Task OnConnectedAsync()
    {
        try
        {
            if (!IsValidTokenType())
            {
                _logger.LogWarning("Invalid token type. Connection rejected. ConnectionId: {ConnectionId}", Context.ConnectionId);
                Context.Abort();
                return;
            }

            var currentUser = GetCurrentUser();
            var userId = currentUser.GetId();
            
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("User connected without valid Id claim. ConnectionId: {ConnectionId}", Context.ConnectionId);
                Context.Abort();
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            _logger.LogInformation("User {UserId} connected to NotificationHub with connection {ConnectionId}", userId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user ID during connection. ConnectionId: {ConnectionId}", Context.ConnectionId);
            Context.Abort();
            return;
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var currentUser = GetCurrentUser();
            var userId = currentUser.GetId();
            
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("User disconnected without valid Id claim. ConnectionId: {ConnectionId}", Context.ConnectionId);
                await base.OnDisconnectedAsync(exception);
                return;
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            _logger.LogInformation("User {UserId} disconnected from NotificationHub. ConnectionId: {ConnectionId}", userId, Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during disconnect for connection {ConnectionId}", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
