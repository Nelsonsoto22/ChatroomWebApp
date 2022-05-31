using Microsoft.AspNetCore.SignalR;

namespace ChatroomWebApp.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    public async Task SendMessageGroup(string group, string user, string message)
    {
        await Clients.Groups(group).SendAsync("ReceiveMessage", user, message);
    }
}
