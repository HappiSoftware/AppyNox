using AppyNox.Services.Sso.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AppyNox.Services.Sso.Server.UI.Hubs;

public class ServerHubWrapper(IHubContext<ServerHub, ISignalRHub> hubContext) : IApplicationHubWrapper
{
    private readonly IHubContext<ServerHub, ISignalRHub> _hubContext = hubContext;

    public async Task JobStarted(string message)
    {
        await _hubContext.Clients.All.Start(message);
    }

    public async Task JobCompleted(string message)
    {
        await _hubContext.Clients.All.Completed(message);
    }
}