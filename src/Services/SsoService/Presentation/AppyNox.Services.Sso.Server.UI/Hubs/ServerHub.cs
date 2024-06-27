using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace AppyNox.Services.Sso.Server.UI.Hubs;

[Authorize(AuthenticationSchemes = "Identity.Application")]
public class ServerHub : Hub<ISignalRHub>
{
    private static readonly ConcurrentDictionary<string, string> OnlineUsers = new();
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var username = Context.User?.Identity?.Name ?? string.Empty;
        // Notify all clients if this is a new user connecting.
        if (!OnlineUsers.Any(x => x.Value == username))
        {
            await Clients.All.Connect(connectionId, username);
        }
        if (!OnlineUsers.ContainsKey(connectionId))
        {
            OnlineUsers.TryAdd(connectionId, username);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        // Remove the connection and check if it was the last one for this user.
        if (OnlineUsers.TryRemove(connectionId, out var username))
        {
            if (!OnlineUsers.Any(x => x.Value == username))
            {
                await Clients.All.Disconnect(connectionId, username);
            }
        }
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string message)
    {
        var username = Context.User?.Identity?.Name ?? string.Empty;
        await Clients.All.SendMessage(username, message);
    }

    public async Task SendPrivateMessage(string to, string message)
    {
        var username = Context.User?.Identity?.Name ?? string.Empty;
        await Clients.User(to).SendPrivateMessage(username, to, message);
    }

    public async Task SendNotification(string message)
    {
        await Clients.All.SendNotification(message);
    }

    public async Task Completed(string message)
    {
        await Clients.All.Completed(message);
    }
}