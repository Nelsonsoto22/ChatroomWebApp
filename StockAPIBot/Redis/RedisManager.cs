using StackExchange.Redis;

namespace StockAPIBot.Redis;

public interface IRedisManager
{
    Task<bool> sendMessage(string channel, string message);
}

public class RedisManager : IRedisManager
{
    ConnectionMultiplexer _redis;

    public RedisManager()
    {
        try
        {
            _redis = ConnectionMultiplexer.Connect("localhost:6379");

        }
        catch (Exception)
        {
            _redis = null;
        }
    }

    public async Task<bool> sendMessage(string channel, string message)
    {
        if (_redis != null)
        {
            var subscriber = _redis.GetSubscriber();
            await subscriber.PublishAsync(channel, message);
            return true;
        }
        else
            return false;


    }


}
