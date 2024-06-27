using Microsoft.AspNetCore.SignalR;

namespace AppyNox.Services.Sso.Infrastructure.Hubs;

public class NoxHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}