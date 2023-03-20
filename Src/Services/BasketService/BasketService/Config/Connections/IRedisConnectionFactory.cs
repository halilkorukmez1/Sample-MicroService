using StackExchange.Redis;

namespace BasketService.Config.Connections;
public interface IRedisConnectionFactory
{
    ConnectionMultiplexer Connection();
    IDatabase GetDatabase(int db = 1);
}
