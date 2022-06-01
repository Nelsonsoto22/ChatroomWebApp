using ChatroomWebApp.Redis;
using Microsoft.AspNetCore.SignalR;

namespace ChatroomWebApp.Hubs;

public interface IChatHub
{
    Task SendMessage(string user, string message);
}

public class ChatHub : Hub, IChatHub
{

    public ChatHub()
    {
        RedisClient redisClient = new RedisClient();

        redisClient.subscribeToChannel("stockChannel", (message) => {
            Clients.All.SendAsync("ReceiveMessage", "stock", message);
        });
    }
    public async Task SendMessage(string user, string message)
    {
        if (message != null)
        {
            if (message.StartsWith("/stock="))
            {
                string[] stockSplitted = message.Split('=');
                string stockMessage = message;
                if (stockSplitted.Length > 1)
                    stockMessage = await getStockMessage(stockSplitted[1]);
                message = stockMessage;
            }

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
    public async Task SendMessageGroup(string group, string user, string message)
    {
        await Clients.Groups(group).SendAsync("ReceiveMessage", user, message);
    }

    private async Task<string> getStockMessage(string stock_code)
    {
        var stockURL = $"https://localhost:7152/api/stockbot/{stock_code}";

        var client = new HttpClient();
        HttpContent httpContent = new StringContent(stock_code);

        HttpResponseMessage response = await client.PostAsync(stockURL, httpContent);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }
}
