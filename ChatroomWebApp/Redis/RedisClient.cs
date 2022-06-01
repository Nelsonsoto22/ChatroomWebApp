using StackExchange.Redis;

namespace ChatroomWebApp.Redis;

public class RedisClient
{
    ConnectionMultiplexer _redis;
    

    public RedisClient()
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

    public void subscribeToChannel(string channel, Action<string> action)
    {
        if (_redis != null)
        {
            var subscriber = _redis.GetSubscriber();
            //subscriber.SubscribeAsync(channel, (redisChannel, message) => {
            //    action(message);
            //});
        }
    }
}
