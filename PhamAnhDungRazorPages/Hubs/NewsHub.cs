using Microsoft.AspNetCore.SignalR;

namespace PhamAnhDungRazorPages.Hubs;

public class NewsHub : Hub
{
    public async Task SendUpdate(string action, object data)
    {
        await Clients.All.SendAsync("NewsUpdated", action, data);
    }
}